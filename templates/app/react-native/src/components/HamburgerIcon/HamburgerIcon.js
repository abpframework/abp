import { Ionicons } from '@expo/vector-icons';
import { Icon } from 'native-base';
import React from 'react';
import { Platform } from 'react-native';

export default function HamburgerIcon({ navigation, ...iconProps }) {
  return (
    <Icon
      onPress={() => navigation.openDrawer()}
      as={Ionicons}
      name={Platform.OS ? 'ios-menu' : 'md-menu'}
      size="8"
      marginLeft={2}
      {...iconProps}
    />
  );
}
