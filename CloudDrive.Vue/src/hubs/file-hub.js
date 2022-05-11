import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";

export default {
  install(Vue) {
    const connection = new HubConnectionBuilder()
      .withUrl("https://localhost:44390/file-hub")
      .configureLogging(LogLevel.Information)
      .build();

    connection.on("FileAdded", (id, fileName) => {
      Vue.config.globalProperties.$mitt.emit("file-added", { id, fileName });
    });

    connection.start();
  },
};
