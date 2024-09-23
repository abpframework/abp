export const VOLO_REGEX = /^Volo\.Abp\.(Application\.Dtos|ObjectExtending)/;
export const VOLO_REMOTE_STREAM_CONTENT = [
  'Volo.Abp.Content.IRemoteStreamContent',
  'Volo.Abp.Content.RemoteStreamContent',
];

export const SAAS_NAMESPACE = 'Volo.Saas';
export const TENANT_KEY = 'tenant';

export const VOLO_PACKAGE_PROXY_IMPORTS = new Map<string, string>([
  ['Volo.Abp.FileManagement.FileDescriptorDto', '@volo/abp.ng.file-management/proxy'],
  ['Volo.Abp.FileManagement.DirectoryContentDto', '@volo/abp.ng.file-management/proxy'],
  ['Volo.Abp.AuditLogging.AuditLogDto', '@volo/abp.ng.audit-logging/proxy'],
  ['Volo.Abp.AuditLogging.AuditLogActionDto', '@volo/abp.ng.audit-logging/proxy'],
  ['Volo.Abp.AuditLogging.EntityChangeDto', '@volo/abp.ng.audit-logging/proxy'],
  ['Volo.Abp.AuditLogging.EntityPropertyChangeDto', '@volo/abp.ng.audit-logging/proxy'],
  ['Volo.Chat.Messages', '@volo/abp.ng.chat/proxy'],
  ['Volo.Chat.ChatMessageDto', '@volo/abp.ng.chat/proxy'],
  ['Volo.Chat.ChatConversationDto', '@volo/abp.ng.chat/proxy'],
  ['Volo.Abp.Gdpr.GdprRequestDto', '@volo/abp.ng.gdpr/proxy'],
  ['Volo.Saas.Tenants.TenantDto', '@volo/abp.ng.saas/proxy'],
  //TenantConnectionStringDto it must end with Strings.
  ['Volo.Saas.Tenants.TenantConnectionStringDto', '@volo/abp.ng.saas/proxy'],
  ['Volo.Saas.Editions.EditionDto', '@volo/abp.ng.saas/proxy'],
  [
    'Volo.Abp.TextTemplateManagement.TextTemplates.TextTemplateContentDto',
    '@volo/abp.ng.text-template-management/proxy',
  ],
  [
    'Volo.Abp.TextTemplateManagement.TextTemplates.TemplateDefinitionDto',
    '@volo/abp.ng.text-template-management/proxy',
  ],
]);
