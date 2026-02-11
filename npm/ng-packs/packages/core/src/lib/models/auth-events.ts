export type EventType =
  | 'discovery_document_loaded'
  | 'jwks_load_error'
  | 'invalid_nonce_in_state'
  | 'discovery_document_load_error'
  | 'discovery_document_validation_error'
  | 'user_profile_loaded'
  | 'user_profile_load_error'
  | 'token_received'
  | 'token_error'
  | 'code_error'
  | 'token_refreshed'
  | 'token_refresh_error'
  | 'silent_refresh_error'
  | 'silently_refreshed'
  | 'silent_refresh_timeout'
  | 'token_validation_error'
  | 'token_expires'
  | 'session_changed'
  | 'session_error'
  | 'session_terminated'
  | 'session_unchanged'
  | 'logout'
  | 'popup_closed'
  | 'popup_blocked'
  | 'token_revoke_error';

export abstract class AuthEvent {
  constructor(public readonly type: EventType) {
    this.type = type;
  }
}

export class AuthSuccessEvent extends AuthEvent {
  constructor(public readonly type: EventType, public readonly info?: any) {
    super(type);
  }
}

export class AuthInfoEvent extends AuthEvent {
  constructor(public readonly type: EventType, public readonly info?: any) {
    super(type);
  }
}

export class AuthErrorEvent extends AuthEvent {
  constructor(
    public readonly type: EventType,
    public readonly reason: object,
    public readonly params?: object,
  ) {
    super(type);
  }
}
