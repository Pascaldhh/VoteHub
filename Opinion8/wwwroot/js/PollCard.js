import { connection } from "./connections.js";

export default class PollCard extends HTMLElement {
  constructor() {
    super();
  }
  connectedCallback() {
    this.attachShadow({ mode: "open" });

    const template = document.getElementById("poll-card-template");
    const content = template.content;

    this.shadowRoot.appendChild(content.cloneNode(true));

    this.setSlots();
    this.setTailwind();

    this.events();
  }

  setTailwind() {
    const styleSheets = document.styleSheets;
    const tailwindcss = Object.values(styleSheets).find(
      (sheet) => sheet.ownerNode.id === "tailwindcss",
    );

    if (!tailwindcss) return;
    const tailwindcssSheet = new CSSStyleSheet();
    tailwindcssSheet.replaceSync(
      [...tailwindcss.cssRules].map((rule) => rule.cssText).join(" "),
    );

    this.shadowRoot.adoptedStyleSheets = [tailwindcssSheet];
  }

  setValues(poll) {
    console.log(poll);

    const questionSlot = this.shadowRoot.querySelector(`slot[name='question']`);
    if (questionSlot)
      questionSlot.textContent = poll.question ?? "[No question]";

    const optionsSlot = this.shadowRoot.querySelector("slot[name='options']");
    if (optionsSlot) {
      const options = poll.options ?? "[No options]";
      if (options) {
        const inputOptions = options
          .map(
            (option) =>
              `<label><input type="radio" name="voteOption"/> ${option}</label>`,
          )
          .join("");

        optionsSlot.innerHTML = `<div class="flex flex-col"> ${inputOptions}</div>`;
      }
    }

    const votersSlot = this.shadowRoot.querySelector("slot[name='voters']");
    if (votersSlot) votersSlot.textContent = poll.voters ?? "";
  }

  setSlots() {
    this.setValues({
      question: this.getAttribute("question"),
      options: JSON.parse(this.getAttribute("options") ?? ""),
      voters: this.getAttribute("voters"),
    });
  }

  vote() {
    const id = this.dataset.pollId;
    if (!id && !/^\d+$/.test(id)) return;

    connection.invoke("PollVote", parseInt(id));
  }

  update(poll) {
    this.setValues({
      ...poll,
      options: poll.options.map((option) => option.name),
    });
  }

  events() {
    this.shadowRoot
      .querySelector("#vote")
      ?.addEventListener("click", this.vote.bind(this));
  }
}
