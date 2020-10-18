<template>
  <div class="container">
    <div class="tile is-ancestor">
      <div class="tile is-parent is-8">
        <article class="tile is-child box border"><!-- QUESTION TILE -->
          <p class="title" id="questionNumber">Question 000</p>
          <p class="subtitle" id="questionTopic">Topic: ---</p>
          <div class="content">
            <p class="is-size-3-tablet" id="questionBox"></p>
          </div>
        </article>
      </div>
      <div class="tile is-parent is-vertical"> <!-- SCORE AND LIFELINE TILES -->
        <article class="tile is-child box border">
          <p class="title is-centered">Lifelines</p>
          <p class="subtitle is-centered">Need some help? This is the place!</p>
          <div class="buttons is-centered">
            <b-button class="lifeline is-yellow">50/50</b-button>
            <b-button class="lifeline is-yellow">Skip Question</b-button>
          </div>
        </article>
        <article class="tile is-child box border">
          <p class="title is-centered" id="playerPosition">---</p>
          <p class="subtitle is-centered" id="playerScore">Score: ---</p>
        </article>
      </div>
    </div>
    <div class="tile is-ancestor">
      <div class="tile is-parent is-vertical buttons">
        <b-button @click="checkAnswer('ansOne')" class="tile is-size-3-tablet is-child field is-grouped is-white">
          <p class="is-size-3-tablet answerLabel" id="ansOne">----</p>
        </b-button>
        <b-button @click="checkAnswer('ansTwo')" class="tile is-size-3-tablet is-child field is-grouped is-white">
          <p class="is-size-3-tablet answerLabel" id="ansTwo">----</p>
        </b-button>
      </div>
      <div class="tile is-parent is-vertical buttons">
        <b-button @click="checkAnswer('ansThree')" class="tile is-size-3-tablet is-child field is-grouped is-white">
          <p class="is-size-3-tablet answerLabel" id="ansThree">----</p>
        </b-button>
        <b-button @click="checkAnswer('ansFour')" class="tile is-size-3-tablet is-child field is-grouped is-white">
          <p class="is-size-3-tablet answerLabel" id="ansFour">----</p>
        </b-button>
      </div>
      <div class="tile is-parent is-4"> <!-- LEADERBOARD TILE -->
        <article class="tile is-child box border">
          <p class="subtitle is-centered">Wondering how your competitors are doing? This is the place for you 👀</p>
          <div class="message">
            <div class="message-header">
              <p>Leaderboard</p>
            </div>
            <div class="message-body">
              <b-table
                :data="tableData"
                :columns="tableColumns"
                :perPage="5"
                paginated
                default-sort="score"
                default-sort-direction="desc"
                :paginationSimple="false"
              ></b-table>
            </div>
          </div>
        </article>
      </div>
    </div>
    <!--<button @click="getQsTokens">Refresh Data</button>
    <div>
      <h1>Lobby info</h1>
      <p v-if="$fetchState.pending">Fetching lobby details ...</p>
      <p v-else-if="$fetchState.error">
        Error while fetching lobby details: {{ $fetchState.error.message }}
      </p>
      <ul v-else>
        <p>
          {{ this.lobbyInfo }}
        </p>
      </ul>
    </div>-->

  </div> <!-- closing container tag-->
</template>

<script>
export default {
  name: 'Game',
  metaInfo: {
    meta: [
      { charset: 'utf-8' },
      { name: 'viewport', content: 'width=device-width, initial-scale=1' }
    ]
  },
  data() {
    return {
        tableData: [],
        currQuestion: 1,
        currQuestionJSON: null,
        lifeline5050: true,
        lifelineSkip: true,
        nickname: "",
        score: 0,
        tableColumns: [
            {
                field: 'name',
                label: 'Nickname',
                width: '40',
            },
            {
                field: 'score',
                label: 'Score',
                numeric: true,
                sortable: true,
                centered: true
            }
        ],
      lobbyInfo: []
    }
  },
  methods: {
    startGame() {
      console.log("YAY");
      this.getQs();
    },
    getGameDetails() {
      let allCookies = document.cookie;
      let cookieArr = allCookies.split('; ');
      let nickname;
      let lobbyId;
      let cookie;
      for (cookie of cookieArr) {
        if (cookie.includes("nickname")) {
          nickname = cookie.split("nickname=")[1];
        }
        if (cookie.includes("lobbyId")) {
          lobbyId = cookie.split("lobbyId=")[1];
        }
      }
      this.nickname = nickname;
      return lobbyId;
    },
    checkAnswer(buttonId) {
      console.info(document.getElementById(buttonId).innerHTML)
      if (document.getElementById(buttonId).innerHTML == this.currQuestionJSON.correct_answer) {
        alert("Correct answer! ✔");
        this.updatePlayerScoreAndPos(500);
      }
      else {
        alert("Wrong answer! ❌");
        this.updatePlayerScoreAndPos(-500);
      }
      this.getNextQuestion();
    },
    async updateLobbyTable() {
      let tempLobbyInfo = {
        id: this.lobbyInfo.id,
        easyQuestions: JSON.stringify(this.lobbyInfo.easyQuestions),
        mediumQuestions: JSON.stringify(this.lobbyInfo.mediumQuestions),
        hardQuestions: JSON.stringify(this.lobbyInfo.hardQuestions),
      };
      const response = await fetch(`/quizApi/Lobbies/${this.lobbyInfo.id}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(this.lobbyInfo)
      }).then((res) => res.json());
      console.info("IN UPDATE LOBBY");
      console.info(response);
    },
    async updatePlayerTable(){
      let playerInfo = {
        name: this.nickname,
        Score: this.score,
        lobbyId: this.lobbyInfo.id,
        questionIndex: this.currQuestion,
        Lifeline5050: this.lifeline5050,
        LifelineSkip: this.lifelineSkip
      };
      console.info(playerInfo);
      const response = await fetch('/quizApi/Players', {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(playerInfo)
      }).then((res) => res.json());
      console.info(response);
    },
    updatePlayerScoreAndPos(adjustment) {
      this.score += adjustment;
      document.getElementById("playerScore").innerHTML = `Score: ${this.score}`;
      this.refreshLeaderboard();
      this.updatePlayerTable();
      // this.tableData.push({id: 5050, lifeline5050: true, lifelineSkip: true, lobbyId: 90909090, name: "bethany", score: this.score, questionIndex: this.currQuestion}); // TEMPORARY FIX !! TODO: REMOVE THIS LATER after full API integration
      this.tableData.sort((a,b) => b.score - a.score);
      let playerPos = 0;
      for (playerPos=0; playerPos < this.tableData.length; playerPos++) {
        console.info("in player loop!");
        if (this.tableData[playerPos].name === this.nickname) {
            break;
        }
      }
      playerPos++;
      let positionElem = document.getElementById("playerPosition");
      console.info(`Player position: ${playerPos}`);
      switch (playerPos) {
        case 1:
          positionElem.innerHTML = "1st";
          break;
        case 2:
          positionElem.innerHTML = "2nd";
          break;
        case 3:
          positionElem.innerHTML = "3rd";
          break;
        default:
          positionElem.innerHTML = `${playerPos}th`;
          break;
      }
    },
    getNextQuestion() {
      this.currQuestion++;
      this.updatePlayerTable();
      this.loadQs();
    },
    async refreshLeaderboard() {
      this.tableData = await fetch(`/quizApi/Players/inlobby/${this.lobbyInfo.id}`).then((res) => res.json());
      console.info(this.tableData);
    },
    async getQs() {
      this.lobbyInfo = await fetch(`/quizApi/Lobbies/${this.lobbyInfo.id}`).then((res) => res.json());
      console.info("IN HERE");
      if (this.lobbyInfo.easyQuestions === "" || this.lobbyInfo.mediumQuestions === "" || this.lobbyInfo.hardQuestions === "") { // checks the DB lobby data to see if questions have already been generated, if not, it generates them
        this.lobbyInfo.easyQuestions = await fetch(`/getQuestions/amount=10&category=9&difficulty=easy&type=multiple&encode=base64`).then((res) => res.json()).then((res) => res.results); // using the tokens to get the questions via the API
        this.lobbyInfo.mediumQuestions = await fetch(`/getQuestions/amount=10&category=9&difficulty=medium&type=multiple&encode=base64`).then((res) => res.json()).then((res) => res.results); // using the tokens to get the questions via the API
        this.lobbyInfo.hardQuestions = await fetch(`/getQuestions/amount=10&category=9&difficulty=hard&type=multiple&encode=base64`).then((res) => res.json()).then((res) => res.results); // using the tokens to get the questions via the API
        this.updateLobbyTable();
      }
      /*this.easyQs = await fetch(`/getQuestions/amount=10&category=9&difficulty=easy&type=multiple&encode=base64`).then((res) => res.json()).then((res) => res.results); // using the tokens to get the questions via the API
      this.mediumQs = await fetch(`/getQuestions/amount=10&category=9&difficulty=medium&type=multiple&encode=base64`).then((res) => res.json()).then((res) => res.results);
      this.hardQs = await fetch(`/getQuestions/amount=10&category=9&difficulty=hard&type=multiple&encode=base64`).then((res) => res.json()).then((res) => res.results);*/
      console.info("Base64 questions");

      this.loadQs();
    },
    decodeJsonData() {  // had issues with HTML encoding so this converts the Base64 encoded data back into ASCII
      let tempVar = this.currQuestionJSON;
      this.currQuestionJSON.category = decodeURIComponent(escape(window.atob(tempVar.category)));
      this.currQuestionJSON.correct_answer = decodeURIComponent(escape(window.atob(tempVar.correct_answer)));
      this.currQuestionJSON.difficulty = decodeURIComponent(escape(window.atob(tempVar.difficulty)));
      this.currQuestionJSON.question = decodeURIComponent(escape(window.atob(tempVar.question)));
      this.currQuestionJSON.type = decodeURIComponent(escape(window.atob(tempVar.type)));
      let incorrectAnswers = [];
      let answer;
      for (answer of tempVar.incorrect_answers) {
        incorrectAnswers.push(decodeURIComponent(escape(window.atob(answer))));
      }
      this.currQuestionJSON.incorrect_answers = incorrectAnswers;
      console.info(this.currQuestionJSON);
    },
    loadQs() {
      console.info(`current question: ${this.currQuestion - 1}`);
      if (this.currQuestion < 10) {
        this.currQuestionJSON = this.lobbyInfo.easyQuestions[this.currQuestion -1];
      }
      else if (this.currQuestion < 20) {
        this.currQuestionJSON = this.lobbyInfo.mediumQuestions[this.currQuestion -10];
      }
      else if (this.currQuestion < 30) {
        this.currQuestionJSON = this.lobbyInfo.hardQuestions[this.currQuestion -20];
      }
      else if (this.currQuestion == 30) { // TODO: end of game
        alert("Game over!");
        const allAnsButtons = document.getElementsByClassName("answerButton");
        let ansButton;
        for (ansButton of allAnsButtons) {
          ansButton.disabled = true;
        }
        return 0;
      }
      this.decodeJsonData();
      document.getElementById("questionNumber").innerHTML = `Question ${this.currQuestion}`;  // updates the question number and the topic
      document.getElementById("questionTopic").innerHTML = `Difficulty: ${this.currQuestionJSON.difficulty} <br>Topic: ${this.currQuestionJSON.category} </br>`
      this.updateQuestion();
    },
    updateQuestion() { // pick a random number between 1 and 4, this will be used to asign the correct answer to a button.
      document.getElementById("questionBox").innerHTML = this.currQuestionJSON.question;  // updates the question box and shows the question to the player
      const correctAnswer = this.getRandomNum(0,3);
      const answerLabels = document.getElementsByClassName("answerLabel");
      answerLabels[correctAnswer].innerHTML = this.currQuestionJSON.correct_answer;
      console.info(`correct answer - ${this.currQuestionJSON.correct_answer}`);
      let counter = 0;
      for (let i = 0; i < answerLabels.length; i++) { // assigns the other answers to the remaining buttons
        if (answerLabels[i].innerHTML != this.currQuestionJSON.correct_answer) {
          answerLabels[i].innerHTML = this.currQuestionJSON.incorrect_answers[counter];
          console.info(`incorrect answer - ${this.currQuestionJSON.incorrect_answers[counter]}`);
          counter++;
        }
      }
    },
    getRandomNum (min, max) { // SOURCE: https://www.freecodecamp.org/news/how-to-use-javascript-math-random-as-a-random-number-generator/
      return Math.floor(Math.random() * (max - min + 1)) + min;
    }
  },
  async fetch() {
    let lobbyId = this.getGameDetails();
    this.lobbyInfo = await fetch(`/quizApi/Lobbies/${lobbyId}`).then((res) => res.json());
    console.info(this.lobbyInfo);

    document.getElementById("lobbyCode").innerHTML = this.lobbyInfo.id;
    document.getElementById("userNickname").innerHTML = this.nickname;
    this.updatePlayerScoreAndPos(0);
    this.startGame();
  },
}

</script>


<style lang="scss">
  $warning: #ffba49;
  $link: #20a39e;
  $info: #a4a9ad;
    $danger: #f9b1b1;
  $primary: #23001e;

  .answerLabel {
      white-space: break-spaces;

  }

  .border{
    border: 3px solid black;
  }

  .tile{
    width: 100%;
  }

  .is-yellow{
    background-color: #ffba49;
    color: black;
    text-align: center;
  }

  .lifeline{
    width: 170px;
    height: 100px;
    font-size: 25px;;
  }

  .is-centered{
    text-align: center;
  }

</style>
