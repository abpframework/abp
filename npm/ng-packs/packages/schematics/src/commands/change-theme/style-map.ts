import { ThemeOptionsEnum } from './theme-options.enum';

export type StyleDefinition =
  | {
      input: string;
      inject: boolean;
      bundleName: string;
    }
  | string;

export type StyleMapType = {
  [key: string]: StyleDefinition[];
};

export const styleMap: StyleMapType = {
  [ThemeOptionsEnum.basic]: [
    {
      input: 'node_modules/bootstrap/dist/css/bootstrap.rtl.min.css',
      inject: false,
      bundleName: 'bootstrap-rtl.min',
    },
    {
      input: 'node_modules/bootstrap/dist/css/bootstrap.min.css',
      inject: true,
      bundleName: 'bootstrap-ltr.min',
    },
  ],
  [ThemeOptionsEnum['leptonx-lite']]: [
    {
      input: 'node_modules/@volo/ngx-lepton-x.lite/assets/css/bootstrap-dim.css',
      inject: false,
      bundleName: 'bootstrap-dim',
    },
    {
      input: 'node_modules/@volo/ngx-lepton-x.lite/assets/css/ng-bundle.css',
      inject: false,
      bundleName: 'ng-bundle',
    },
    {
      input: 'node_modules/@volo/ngx-lepton-x.lite/assets/css/side-menu/layout-bundle.css',
      inject: false,
      bundleName: 'layout-bundle',
    },
    {
      input: 'node_modules/@abp/ng.theme.lepton-x/assets/css/abp-bundle.css',
      inject: false,
      bundleName: 'abp-bundle',
    },
    {
      input: 'node_modules/@volo/ngx-lepton-x.lite/assets/css/bootstrap-dim.rtl.css',
      inject: false,
      bundleName: 'bootstrap-dim.rtl',
    },
    {
      input: 'node_modules/@volo/ngx-lepton-x.lite/assets/css/ng-bundle.rtl.css',
      inject: false,
      bundleName: 'ng-bundle.rtl',
    },
    {
      input: 'node_modules/@volo/ngx-lepton-x.lite/assets/css/side-menu/layout-bundle.rtl.css',
      inject: false,
      bundleName: 'layout-bundle.rtl',
    },
    {
      input: 'node_modules/@abp/ng.theme.lepton-x/assets/css/abp-bundle.rtl.css',
      inject: false,
      bundleName: 'abp-bundle.rtl',
    },
  ],
  [ThemeOptionsEnum.lepton]: [
    {
      input: 'node_modules/@volo/abp.ng.theme.lepton/dist/global/styles/lepton1.min.css',
      inject: false,
      bundleName: 'lepton1',
    },
    {
      input: 'node_modules/@volo/abp.ng.theme.lepton/dist/global/styles/lepton2.min.css',
      inject: false,
      bundleName: 'lepton2',
    },
    {
      input: 'node_modules/@volo/abp.ng.theme.lepton/dist/global/styles/lepton3.min.css',
      inject: false,
      bundleName: 'lepton3',
    },
    {
      input: 'node_modules/@volo/abp.ng.theme.lepton/dist/global/styles/lepton4.min.css',
      inject: false,
      bundleName: 'lepton4',
    },
    {
      input: 'node_modules/@volo/abp.ng.theme.lepton/dist/global/styles/lepton5.min.css',
      inject: false,
      bundleName: 'lepton5',
    },
    {
      input: 'node_modules/@volo/abp.ng.theme.lepton/dist/global/styles/lepton6.min.css',
      inject: false,
      bundleName: 'lepton6',
    },
    {
      input: 'node_modules/@volo/abp.ng.theme.lepton/dist/global/styles/lepton1.rtl.min.css',
      inject: false,
      bundleName: 'lepton1.rtl',
    },
    {
      input: 'node_modules/@volo/abp.ng.theme.lepton/dist/global/styles/lepton2.rtl.min.css',
      inject: false,
      bundleName: 'lepton2.rtl',
    },
    {
      input: 'node_modules/@volo/abp.ng.theme.lepton/dist/global/styles/lepton3.rtl.min.css',
      inject: false,
      bundleName: 'lepton3.rtl',
    },
    {
      input: 'node_modules/@volo/abp.ng.theme.lepton/dist/global/styles/lepton4.rtl.min.css',
      inject: false,
      bundleName: 'lepton4.rtl',
    },
    {
      input: 'node_modules/@volo/abp.ng.theme.lepton/dist/global/styles/lepton5.rtl.min.css',
      inject: false,
      bundleName: 'lepton5.rtl',
    },
    {
      input: 'node_modules/@volo/abp.ng.theme.lepton/dist/global/styles/lepton6.rtl.min.css',
      inject: false,
      bundleName: 'lepton6.rtl',
    },
  ],
  [ThemeOptionsEnum.leptonx]: [
    {
      input: 'node_modules/@volosoft/ngx-lepton-x/assets/css/dark.css',
      inject: false,
      bundleName: 'dark',
    },
    {
      input: 'node_modules/@volosoft/ngx-lepton-x/assets/css/light.css',
      inject: false,
      bundleName: 'light',
    },
    {
      input: 'node_modules/@volosoft/ngx-lepton-x/assets/css/dim.css',
      inject: false,
      bundleName: 'dim',
    },
    {
      input: 'node_modules/@volosoft/ngx-lepton-x/assets/css/bootstrap-dim.css',
      inject: false,
      bundleName: 'bootstrap-dim',
    },
    {
      input: 'node_modules/@volosoft/ngx-lepton-x/assets/css/bootstrap-dark.css',
      inject: false,
      bundleName: 'bootstrap-dark',
    },
    {
      input: 'node_modules/@volosoft/ngx-lepton-x/assets/css/bootstrap-light.css',
      inject: false,
      bundleName: 'bootstrap-light',
    },
    {
      input: 'node_modules/@volosoft/ngx-lepton-x/assets/css/ng-bundle.css',
      inject: false,
      bundleName: 'ng-bundle',
    },
    {
      input: 'node_modules/@volosoft/ngx-lepton-x/assets/css/side-menu/layout-bundle.css',
      inject: false,
      bundleName: 'layout-bundle',
    },
    {
      input: 'node_modules/@volosoft/abp.ng.theme.lepton-x/assets/css/abp-bundle.css',
      inject: false,
      bundleName: 'abp-bundle',
    },
    {
      input: 'node_modules/@volosoft/ngx-lepton-x/assets/css/dark.rtl.css',
      inject: false,
      bundleName: 'dark.rtl',
    },
    {
      input: 'node_modules/@volosoft/ngx-lepton-x/assets/css/light.rtl.css',
      inject: false,
      bundleName: 'light.rtl',
    },
    {
      input: 'node_modules/@volosoft/ngx-lepton-x/assets/css/dim.rtl.css',
      inject: false,
      bundleName: 'dim.rtl',
    },
    {
      input: 'node_modules/@volosoft/ngx-lepton-x/assets/css/bootstrap-dim.rtl.css',
      inject: false,
      bundleName: 'bootstrap-dim.rtl',
    },
    {
      input: 'node_modules/@volosoft/ngx-lepton-x/assets/css/bootstrap-dark.rtl.css',
      inject: false,
      bundleName: 'bootstrap-dark.rtl',
    },
    {
      input: 'node_modules/@volosoft/ngx-lepton-x/assets/css/bootstrap-light.rtl.css',
      inject: false,
      bundleName: 'bootstrap-light.rtl',
    },
    {
      input: 'node_modules/@volosoft/ngx-lepton-x/assets/css/ng-bundle.rtl.css',
      inject: false,
      bundleName: 'ng-bundle.rtl',
    },
    {
      input: 'node_modules/@volosoft/ngx-lepton-x/assets/css/side-menu/layout-bundle.rtl.css',
      inject: false,
      bundleName: 'layout-bundle.rtl',
    },
    {
      input: 'node_modules/@volosoft/abp.ng.theme.lepton-x/assets/css/abp-bundle.rtl.css',
      inject: false,
      bundleName: 'abp-bundle.rtl',
    },
  ],
};

export const allStyles = Object.values(styleMap).reduce((acc, val) => [...acc, ...val], []);
