import FileHub from "../hubs/file-hub.js";

// "async" is optional;
// more info on params: https://v2.quasar.dev/quasar-cli/boot-files
export default ({ app }) => {
  app.use(FileHub);
};
