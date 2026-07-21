import type { IRemoteStreamContent } from '../../../abp/content/models';
import type { ExtensibleEntityDto } from '@abp/ng.core';

export interface CreateMediaInputWithStream {
  name: string;
  file: IRemoteStreamContent;
}

export interface MediaDescriptorDto extends ExtensibleEntityDto<string> {
  name?: string;
  mimeType?: string;
  size: number;
}
