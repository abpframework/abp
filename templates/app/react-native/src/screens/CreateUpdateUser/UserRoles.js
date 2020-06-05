import React, { useEffect, useState } from 'react';
import PropTypes from 'prop-types';
import { List, ListItem, CheckBox, Body, Text } from 'native-base';
import { TouchableOpacity } from 'react-native';
import { getAllRoles, getUserRoles } from '../../api/IdentityAPI';

function UserRoles({ editingUser = {}, onChangeRoles }) {
  const [roles, setRoles] = useState([]);

  const onPress = index => {
    setRoles(
      roles.map((role, i) => ({
        ...role,
        isSelected: index === i ? !role.isSelected : role.isSelected,
      })),
    );
  };

  useEffect(() => {
    const requests = [getAllRoles()];
    if (editingUser.id) requests.push(getUserRoles(editingUser.id));

    Promise.all(requests).then(([allRoles = [], userRoles = []]) => {
      setRoles(
        allRoles.map(role => ({
          ...role,
          isSelected: editingUser.id
            ? !!userRoles?.find(userRole => userRole?.id === role?.id)
            : role.isDefault,
        })),
      );
    });
  }, []);

  useEffect(() => {
    onChangeRoles(roles.filter(role => role.isSelected).map(role => role.name));
  }, [roles]);

  return (
    <List>
      {roles.map((role, index) => (
        <ListItem key={role.id}>
          <CheckBox checked={role.isSelected} onPress={() => onPress(index)} />
          <Body>
            <TouchableOpacity onPress={() => onPress(index)}>
              <Text>{role.name}</Text>
            </TouchableOpacity>
          </Body>
        </ListItem>
      ))}
    </List>
  );
}

UserRoles.propTypes = {
  editingUser: PropTypes.objectOf(PropTypes.any).isRequired,
  onChangeRoles: PropTypes.func.isRequired,
};

export default UserRoles;
