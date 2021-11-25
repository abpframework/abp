// @flow
import contentTheme from '../components/Content';

export default variables => {
  const override = {
    ...contentTheme(variables),
    backgroundColor: '#f8f8f8',
    '.px20': {
      paddingHorizontal: 20,
    },
  };

  return override;
};
