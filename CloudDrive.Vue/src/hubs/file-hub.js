import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";

const connectToHub = (username, q) => {
  const connection = new HubConnectionBuilder()
    .withUrl(`https://localhost:44390/file-hub?username=${username}`)
    .configureLogging(LogLevel.Information)
    .build();

  connection.on("FileAdded", (id, fileName) => {
    q.notify({
      type: "info",
      message: `Z innego urządzenia został dodany nowy plik ${fileName}`,
    });
  });

  connection.start();
};

export { connectToHub };
