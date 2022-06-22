import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";

const connectToHub = async (username, q) => {
  const connection = new HubConnectionBuilder()
    .withUrl(`http://192.168.55.109:8005/file-hub?username=${username}`, {
      withCredentials: false,
    })
    .configureLogging(LogLevel.Information)
    .build();

  connection.on("FileAdded", (id, fileName) => {
    q.notify({
      type: "positive",
      message: `Plik ${fileName} został dodany pomyślnie!`,
    });
  });

  await connection.start({ withCredentials: false });

  return connection;
};

export { connectToHub };
