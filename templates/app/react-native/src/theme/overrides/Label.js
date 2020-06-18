// @flow
import labelTheme from '../components/Label';

export default variables => {
  const labelOverrides = {
    ...labelTheme(variables),
    '.abpLabel': {
      alignSelf: 'flex-start',
      marginBottom: 5,
      color: '#8F8F8F',
      fontWeight: '600',
      fontSize: 13,
      textTransform: 'uppercase',
    },
  };

  return labelOverrides;
};
