import { useFocusEffect } from '@react-navigation/native';
import i18n from 'i18n-js';
import { connectStyle, Icon, Input, InputGroup, Item, List, Spinner, Text } from 'native-base';
import PropTypes from 'prop-types';
import React, { forwardRef, useCallback, useEffect, useState } from 'react';
import { RefreshControl, StyleSheet, View } from 'react-native';
import LoadingActions from '../../store/actions/LoadingActions';
import { activeTheme } from '../../theme/variables';
import { debounce } from '../../utils/Debounce';
import { connectToRedux } from '../../utils/ReduxConnect';
import LoadingButton from '../LoadingButton/LoadingButton';

function DataList({
  style,
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
    fetch(skipCount + maxResultCount, false).finally(() => setButtonLoading(false));
  };

  useFocusEffect(
    useCallback(() => {
      setSkipCount(0);
      fetch(0, false);
    }, []),
  );

  useEffect(() => {
    function searchFetch() {
      setSearchLoading(true);
      return fetch(0, false).finally(() => setTimeout(() => setSearchLoading(false), 150));
    }
    debounce(searchFetch, debounceTime)();
  }, [filter]);

  return (
    <>
      <Item placeholderLabel style={{ backgroundColor: '#fff' }}>
        <InputGroup style={{ marginLeft: 10 }}>
          <Input
            placeholder={i18n.t('AbpUi::PagerSearch')}
            style={{ padding: 0, margin: 0 }}
            returnKeyType="done"
            value={filter}
            onChangeText={setFilter}
          />
          {searchLoading ? (
            <View>
              <Spinner style={style.spinner} color={style.spinner.color} />
            </View>
          ) : (
            <Icon style={{ fontSize: 20, marginRight: 15 }} name="ios-search" />
          )}
        </InputGroup>
      </Item>
      <View style={style.container}>
        <List
          showsVerticalScrollIndicator
          scrollEnabled
          refreshControl={<RefreshControl refreshing={loading} onRefresh={fetch} />}
          dataArray={records}
          renderRow={(data, sectionID, rowId, ...args) => (
            <>
              {render(data, sectionID, rowId, ...args)}
              {rowId + 1 === skipCount + maxResultCount && totalCount > records.length ? (
                <View style={{ justifyContent: 'center', alignItems: 'center' }}>
                  <LoadingButton loading={buttonLoading} onPress={() => fetchPartial()}>
                    <Text>{i18n.t('AbpUi::LoadMore')}</Text>
                  </LoadingButton>
                </View>
              ) : null}
            </>
          )}
          {...props}
        />
      </View>
    </>
  );
}

DataList.propTypes = {
  ...List.propTypes,
  style: PropTypes.any.isRequired,
  fetchFn: PropTypes.func.isRequired,
  render: PropTypes.func.isRequired,
  maxResultCount: PropTypes.number,
  debounceTime: PropTypes.number,
};

const styles = StyleSheet.create({
  container: { flex: 1 },
  list: {},
  spinner: {
    transform: [{ scale: 0.5 }],
    position: 'absolute',
    right: 8,
    top: -40,
    color: activeTheme.brandInfo,
  },
});

const Forwarded = forwardRef((props, ref) => <DataList {...props} forwardedRef={ref} />);

export default connectToRedux({
  component: connectStyle('ABP.DataList', styles)(Forwarded),
  dispatchProps: {
    startLoading: LoadingActions.start,
    stopLoading: LoadingActions.stop,
  },
});
