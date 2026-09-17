export const APPLICATION_LOCALIZATION_DATA = {
  resources: {
    Default: { texts: {}, baseResources: [] },
    MyProjectName: {
      texts: {
        "'{0}' and '{1}' do not match.": "'{0}' and '{1}' do not match.",
      },
      baseResources: [],
    },
    AbpIdentity: {
      texts: {
        Identity: 'identity',
      },
      baseResources: [],
    },
  },
};

describe('APPLICATION_LOCALIZATION_DATA', () => {
  it('should export localization data', () => {
    expect(APPLICATION_LOCALIZATION_DATA).toBeDefined();
    expect(APPLICATION_LOCALIZATION_DATA.resources).toBeDefined();
  });
});
