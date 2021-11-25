import React from 'react';
import { TouchableOpacity } from 'react-native';
import { Icon } from 'native-base';
import PropTypes from 'prop-types';

function MenuIcon({ onPress, iconName = 'menu' }) {
  return (
    <TouchableOpacity onPress={onPress}>
      <Icon navElement name={iconName} />
    </TouchableOpacity>
  );
}

MenuIcon.propTypes = {
  onPress: PropTypes.func.isRequired,
  iconName: PropTypes.string,
};

export default MenuIcon;
