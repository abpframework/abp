import inputTheme from '../components/Input';

export default variables => {
  const inputOverrides = {
    ...inputTheme(variables),
    '.abpInput': {
      paddingVertical: 0,
      paddingHorizontal: 20,
      borderWidth: 1,
      borderColor: '#e6e6e6',
      borderRadius: 15,
      width: '100%',
      height: 40,
      backgroundColor: '#fff',
      color: '#000',
      lineHeight: 22,
    },
  };

  return inputOverrides;
};
