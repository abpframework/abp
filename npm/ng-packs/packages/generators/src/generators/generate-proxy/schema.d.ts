export interface GenerateProxyGeneratorSchema {
  module: string;
  apiName: string;
  source: string;
  target: string;
  url: string;
  serviceType: string;
  entryPoint: string;
}
