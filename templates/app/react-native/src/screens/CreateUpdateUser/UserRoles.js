import { Box, Checkbox, List } from 'native-base';
import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
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
    <Box w={{base: '100%'}} px="4">
      <List borderWidth={0}>
      {roles.map((role, index) => (
        <List.Item key={role.id} borderBottomWidth={1} >
          <Checkbox isChecked={role.isSelected} onPress={() => onPress(index)} >
          {role.name}
        </Checkbox>
        </List.Item>
      ))}
    </List>
    </Box>
  );
}

UserRoles.propTypes = {
  editingUser: PropTypes.objectOf(PropTypes.any).isRequired,
  onChangeRoles: PropTypes.func.isRequired,
};

export default UserRoles;
