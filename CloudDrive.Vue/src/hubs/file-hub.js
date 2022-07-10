import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";

const connectToHub = async (username, q) => {
  const connection = new HubConnectionBuilder()
    .withUrl(`http://192.168.166.44:8005/file-hub?username=${username}`, {
      withCredentials: false,
    })
    .configureLogging(LogLevel.Information)
    .build();

  connection.serverTimeoutInMilliseconds = 500000;
  connection.keepAliveIntervalInMilliseconds = 500000;
  connection.on("FileAdded", (id, fileName) => {
    q.notify({
      type: "positive",
      message: `Plik ${fileName} został dodany pomyślnie!`,
    });
  });

  await connection.start({ withCredentials: false, transport: "longPolling" });

  return connection;
};

export { connectToHub };
