import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";

const connectToHub = (username, q) => {
  const connection = new HubConnectionBuilder()
    .withUrl(`https://localhost:44390/file-hub?username=${username}`)
    .configureLogging(LogLevel.Information)
    .build();

  connection.on("FileAdded", (id, fileName) => {
    q.notify({
      type: "positive",
      message: `Plik ${fileName} został dodany pomyślnie!`,
    });
  });

  connection.start();

  return connection;
};

export { connectToHub };
