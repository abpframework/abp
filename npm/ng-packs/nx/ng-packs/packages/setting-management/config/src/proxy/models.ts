
export interface EmailSettingsDto {
  smtpHost?: string;
  smtpPort: number;
  smtpUserName?: string;
  smtpPassword?: string;
  smtpDomain?: string;
  smtpEnableSsl: boolean;
  smtpUseDefaultCredentials: boolean;
  defaultFromAddress?: string;
  defaultFromDisplayName?: string;
}

export interface UpdateEmailSettingsDto {
  smtpHost?: string;
  smtpPort: number;
  smtpUserName?: string;
  smtpPassword?: string;
  smtpDomain?: string;
  smtpEnableSsl: boolean;
  smtpUseDefaultCredentials: boolean;
  defaultFromAddress: string;
  defaultFromDisplayName: string;
}
