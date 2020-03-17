import React from 'react';
import { ListItem, Text, Icon, Left, Thumbnail, Body, Fab } from 'native-base';
import { StyleSheet } from 'react-native';
import { getUsers } from '../../api/IdentityAPI';
import { activeTheme } from '../../theme/variables';
import DataList from '../../components/DataList/DataList';
import { withPermission } from '../../hocs/PermissionHOC';

const CreateUserButtonWithPermission = withPermission(Fab, 'AbpIdentity.Users.Create');

function UsersScreen({ navigation }) {
  return (
    <>
      <DataList
        navigation={navigation}
        fetchFn={getUsers}
        render={user => (
          <ListItem avatar onPress={() => navigateToCreateUpdateUserScreen(navigation, user)}>
            <Left style={styles.listItem}>
              <Thumbnail source={require('../../../assets/avatar.png')} />
              <Body>
                <Text>{user.userName}</Text>
                <Text note>{user.email}</Text>
              </Body>
            </Left>
          </ListItem>
        )}
      />

      <CreateUserButtonWithPermission
        containerStyle={{}}
        style={{ backgroundColor: activeTheme.brandPrimary }}
        position="bottomRight"
        onPress={() => navigation.navigate('CreateUpdateUser')}>
        <Icon name="add" />
      </CreateUserButtonWithPermission>
    </>
  );
}

const navigateToCreateUpdateUserScreen = (navigation, user = {}) => {
  navigation.navigate('CreateUpdateUser', {
    userId: user.id,
  });
};

const styles = StyleSheet.create({
  container: { flex: 1 },
  listItem: {
    alignItems: 'center',
    marginVertical: 3,
    marginLeft: -15,
    paddingTop: 5,
    paddingBottom: 5,
    paddingLeft: 15,
    backgroundColor: '#f9f9f9',
    width: '110%',
  },
});

export default UsersScreen;
