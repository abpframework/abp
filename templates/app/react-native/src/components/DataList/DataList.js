import { Ionicons } from '@expo/vector-icons';
import { useFocusEffect } from '@react-navigation/native';
import i18n from 'i18n-js';
import { Box, Center, FlatList, Icon, Input, Spinner, Text } from 'native-base';
import PropTypes from 'prop-types';
import React, { forwardRef, useCallback, useEffect, useState } from 'react';
import { StyleSheet, View } from 'react-native';
import LoadingActions from '../../store/actions/LoadingActions';
import { debounce } from '../../utils/Debounce';
import { connectToRedux } from '../../utils/ReduxConnect';
import LoadingButton from '../LoadingButton/LoadingButton';

function DataList({
  navigation,
  fetchFn,
  render,
  maxResultCount = 15,
  debounceTime = 350,
  ...props
}) {
  const [records, setRecords] = useState([]);
  const [totalCount, setTotalCount] = useState(0);
  const [loading, setLoading] = useState(false);
  const [searchLoading, setSearchLoading] = useState(false);
  const [buttonLoading, setButtonLoading] = useState(false);
  const [skipCount, setSkipCount] = useState(0);
  const [filter, setFilter] = useState('');

  const fetch = (skip = 0, isRefreshingActive = true) => {
    if (isRefreshingActive) setLoading(true);
    return fetchFn({ filter, maxResultCount, skipCount: skip })
      .then(({ items, totalCount: total }) => {
        setTotalCount(total);
        setRecords(skip ? [...records, ...items] : items);
        setSkipCount(skip);
      })
      .finally(() => {
        if (isRefreshingActive) setLoading(false);
      });
  };

  const fetchPartial = () => {
    if (loading || records.length === totalCount) return;

    setButtonLoading(true);
    fetch(skipCount + maxResultCount, false).finally(() =>
      setButtonLoading(false)
    );
  };

  useFocusEffect(
    useCallback(() => {
      setSkipCount(0);
      fetch(0, false);
    }, [])
  );

  useEffect(() => {
    function searchFetch() {
      setSearchLoading(true);
      return fetch(0, false).finally(() =>
        setTimeout(() => setSearchLoading(false), 150)
      );
    }
    debounce(searchFetch, debounceTime)();
  }, [filter]);

  return (
    <Center>
      <Box
        w={{
          base: '95%',
        }}
        mt="2"
      >
        <Input
          placeholder={i18n.t('AbpUi::PagerSearch')}
          style={{ padding: 0, margin: 0 }}
          returnKeyType="done"
          value={filter}
          onChangeText={setFilter}
          InputRightElement={
            searchLoading ? (
              <Spinner color="coolGray.500" marginRight={2} size="sm"/>
            ) : (
              <Icon as={Ionicons} name={'ios-search'} size="4" marginRight={2} color="coolGray.500" />
            )
          }
        />
        <FlatList
          mt="2"
          borderTopWidth="1"
          borderTopColor="#e5e7eb"
          data={records}
          renderItem={(...args) => (
            <>
              {render(...args)}
              {args.index + 1 === skipCount + maxResultCount &&
              totalCount > records.length ? (
                <View
                  style={{ justifyContent: 'center', alignItems: 'center' }}
                >
                  <LoadingButton
                    loading={buttonLoading}
                    onPress={() => fetchPartial()}
                  >
                    <Text>{i18n.t('AbpUi::LoadMore')}</Text>
                  </LoadingButton>
                </View>
              ) : null}
            </>
          )}
          {...props}
        />
      </Box>
    </Center>
  );
}

DataList.propTypes = {
  ...FlatList.propTypes,
  fetchFn: PropTypes.func.isRequired,
  render: PropTypes.func.isRequired,
  maxResultCount: PropTypes.number,
  debounceTime: PropTypes.number,
};

const styles = StyleSheet.create({
  container: { flex: 1 },
  list: {},
});

const Forwarded = forwardRef((props, ref) => (
  <DataList {...props} forwardedRef={ref} />
));

export default connectToRedux({
  component: Forwarded,
  dispatchProps: {
    startLoading: LoadingActions.start,
    stopLoading: LoadingActions.stop,
  },
});
