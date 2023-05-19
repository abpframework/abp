import { Box, HStack, Pressable, Text } from 'native-base';
import React from 'react';
import { getUsers } from '../../api/IdentityAPI';
import DataList from '../../components/DataList/DataList';
import { LocalizationContext } from '../../contexts/LocalizationContext';

function UsersScreen({ navigation }) {
  const { t } = React.useContext(LocalizationContext);

  return (
      <DataList
        navigation={navigation}
        fetchFn={getUsers}
        render={({item}) => (
          <Pressable
          onPress={() => navigateToCreateUpdateUserScreen(navigation, item)}
        >
          <Box
            borderBottomWidth="1"
            borderColor="coolGray.200"
            pl="2"
            pr="5"
            py="2"
          >
            <HStack space={1}>
              <Text color="coolGray.500">{t('AbpIdentity::UserName')}:</Text>
              <Text color="coolGray.800" bold>
                {item.userName}
              </Text>
            </HStack>
            <HStack space={1}>
              <Text color="coolGray.500">{t('AbpIdentity::EmailAddress')}:</Text>
              <Text color="coolGray.800" bold>
                {item.email}
              </Text>
            </HStack>
            <HStack space={1}>
              <Text color="coolGray.500">{t('AbpIdentity::DisplayName:Name')}:</Text>
              <Text color="coolGray.800" bold>
                {item.name}
              </Text>
            </HStack>
            <HStack space={1}>
              <Text color="coolGray.500">{t('AbpIdentity::DisplayName:Surname')}:</Text>
              <Text color="coolGray.800" bold>
                {item.surname}
              </Text>
            </HStack>
          </Box>
        </Pressable>
        )}
      />
  );
}

const navigateToCreateUpdateUserScreen = (navigation, user = {}) => {
  navigation.navigate('CreateUpdateUser', {
    userId: user.id,
  });
};

export default UsersScreen;
