export class ServerCookieParser {
  static parse(cookieHeader: string): { [key: string]: string } {
    const cookies: { [key: string]: string } = {};

    if (!cookieHeader) return cookies;

    try {
      cookieHeader.split(';').forEach(cookie => {
        const parts = cookie.trim().split('=');
        if (parts.length >= 2) {
          const name = parts[0].trim();
          const value = parts.slice(1).join('=');

          if (name) {
            try {
              cookies[name] = decodeURIComponent(value);
            } catch (e) {
              cookies[name] = value;
            }
          }
        }
      });
    } catch (error) {
      console.error('Error parsing cookies:', error);
    }

    return cookies;
  }

  static middleware() {
    return (req: any, res: any, next: any) => {
      req.cookies = ServerCookieParser.parse(req.headers.cookie || '');
      next();
    };
  }

  static getCookie(req: any, name: string): string | undefined {
    const cookieHeader = req.headers.cookie;
    if (!cookieHeader) return undefined;

    const cookies = ServerCookieParser.parse(cookieHeader);
    return cookies[name];
  }
}
