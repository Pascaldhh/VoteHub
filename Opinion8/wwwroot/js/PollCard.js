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
    const questionSlot = this.shadowRoot.querySelector(`slot[name='question']`);
    if (questionSlot)
      questionSlot.textContent = poll.question ?? "[No question]";

    const optionsSlot = this.shadowRoot.querySelector("slot[name='options']");
    const oldInputValues = [...optionsSlot.querySelectorAll("input[name='voteOption']")].map(input => input.value)
    const isSame = oldInputValues.length === poll.options.length ? poll.options.every(option => oldInputValues.find(fOption => fOption == option.id)) : false ;

    if (optionsSlot && !isSame) {
      const options = poll.options ?? "[No options]";
      if (options) {
        const inputOptions = options
          .map(
            (option) =>
              `<div><input class="mr-2" id="${option?.id}" disabled type="radio" name="voteOption" value="${option?.id}"/><label for="${option?.id}">${option?.name}</label></div>`,
          )
          .join("");
        optionsSlot.innerHTML = `<div class="flex flex-col"> ${inputOptions}</div>`;
      }
    }

    if(optionsSlot) {
        const inputs = [...optionsSlot.querySelectorAll("input[name='voteOption']")];
        inputs.forEach(input => {
            if(poll.hasVoted != null) input.disabled = poll.hasVoted;
            const option = poll.options.find(op => input.value == op.id);
            input.nextSibling.textContent = `${option?.name}: ${option?.votes?.length} vote(s)`;
        });
    }

    const votersSlot = this.shadowRoot.querySelector("slot[name='voters']");
    if (votersSlot) votersSlot.textContent = poll?.voteCount ?? "";

    const voteButton = this.shadowRoot.querySelector("#vote");
    if(voteButton && poll.hasVoted != null) voteButton.disabled = poll.hasVoted;


  }

  setSlots() {
    this.setValues({
      question: this.getAttribute("question"),
      options: JSON.parse(this.getAttribute("options") ?? ""),
      voteCount: this.getAttribute("voters"),
      hasVoted: !!this.getAttribute("has-voted")
    });
  }

  vote() {
    const id = this.shadowRoot.querySelector('input[type="radio"][name="voteOption"]:checked')?.value;
    if (!id && !/^\d+$/.test(id)) return;

    connection.invoke("PollVote", parseInt(id));
  }

  update(poll) {
    this.setValues({
      ...poll,
    });
  }

  events() {
    this.shadowRoot
      .querySelector("#vote")
      ?.addEventListener("click", this.vote.bind(this));
  }
}
