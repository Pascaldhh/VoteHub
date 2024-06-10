// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
import { connection } from "./connections.js";
import PollCard from "./PollCard.js";

class Poll {
    static create(poll) {
        console.log(poll)
        const container = document.getElementById("poll-container");
        container.innerHTML += `<poll-card class="max-w-xl w-full" data-poll-id="${poll.id}" question="${poll.question}" options='${JSON.stringify(poll.options)}' voters="${poll.voteCount}" has-voted="${poll.hasVoted ? "true" : ""}"></poll-card>`;
    }
  static update(poll) {
        console.log(poll)
    const pollElement = document.querySelector(`[data-poll-id="${poll.id}"]`);
    pollElement.update(poll);
  }

  static delete(poll) {
    const pollElement = document.querySelector(`[data-poll-id="${poll.id}"]`);
    pollElement.remove();
  }
}

class Site {
  constructor() {
    this.defines();
    connection.start().then(this.start.bind(this));
  }

  start() {
    connection.on("PollCreate", Poll.create);
    connection.on("PollUpdate", Poll.update);
    connection.on("PollDelete", Poll.delete);
  }

  defines() {
    customElements.define("poll-card", PollCard);
  }
}

new Site();
