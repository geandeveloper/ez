import { CapacitorConfig } from '@capacitor/cli';

const config: CapacitorConfig = {
  appId: 'io.ezgym.app',
  appName: 'ezgym-web',
  webDir: 'dist',
  bundledWebRuntime: false,
  server: {
    url: "http://192.168.15.136:4200",
    cleartext: true
  }
};

export default config;
