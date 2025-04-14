import type { Config } from '@jest/types';

const config: Config.InitialOptions = {
  testEnvironment: "jsdom",
  verbose: true,
  preset: "ts-jest",
  transform: {
    "^.+\\.tsx?$": "ts-jest",
    "\\.css$": "<rootDir>/file-transform.ts"
  },
};

export default config;