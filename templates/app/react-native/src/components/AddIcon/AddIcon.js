import { Ionicons } from '@expo/vector-icons';
import { Icon } from 'native-base';
import React from 'react';

export default function AddIcon({ onPress, ...iconProps }) {
  return (
    <Icon
      onPress={onPress}
      as={Ionicons}
      name={'add'}
      size="7"
      marginRight={-2}
      {...iconProps}
    />
  );
}
