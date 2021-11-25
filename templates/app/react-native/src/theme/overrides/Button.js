// @flow
import buttonTheme from '../components/Button';

export default variables => {
  const buttonOverrides = {
    ...buttonTheme(variables),
    '.abpButton': {
      display: 'flex',
      justifyContent: 'center',
      borderRadius: 15,
      width: '100%',
      'NativeBase.Text': {
        fontWeight: '600',
        textTransform: 'uppercase',
      },
    },
  };

  return buttonOverrides;
};
