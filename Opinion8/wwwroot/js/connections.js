export const connection = new signalR.HubConnectionBuilder()
    .withUrl("/Poll")
    .build();
