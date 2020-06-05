import itemTheme from '../components/Item';

export default variables => {
  const itemOverrides = {
    ...itemTheme(variables),
    '.abpInput': {
      paddingVertical: 0,
      paddingHorizontal: 5,
      borderTopWidth: 1,
      borderRightWidth: 1,
      borderBottomWidth: 1,
      borderLeftWidth: 1,
      borderWidth: 1,
      borderColor: '#e6e6e6',
      borderRadius: 15,
      width: '100%',
      height: 40,
      backgroundColor: '#fff',
      color: '#000',
    },
  };

  return itemOverrides;
};
