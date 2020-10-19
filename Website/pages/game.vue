<template>
  <div class="container">
    <!--TIMER CODE TAKEN FROM https://medium.com/js-dojo/how-to-create-an-animated-countdown-timer-with-vue-89738903823f-->
      <div class="base-timer">
        <svg class="base-timer__svg" viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg">
          <g class="base-timer__circle">
            <circle class="base-timer__path-elapsed" cx="50" cy="50" r="45"></circle>
            <path
              :stroke-dasharray="circleDasharray"
              class="base-timer__path-remaining"
              :class="remainingPathColor"
              d="
                M 50, 50
                m -45, 0
                a 45,45 0 1,0 90,0
                a 45,45 0 1,0 -90,0
              "
            ></path>
          </g>
        </svg>
        <span class="base-timer__label">{{ formattedTimeLeft }}</span>
      </div>

    <div class="tile is-ancestor">
      <div class="tile is-parent is-8">
        <article class="tile is-child box border"><!-- QUESTION TILE -->
          <p class="title" id="questionNumber">Question 000</p>
          <p class="subtitle" id="questionTopic">Difficulty: --- <br>Topic: --- <br/></p>
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
          <p class="subtitle is-centered" id="streak">---</p>
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
const FULL_DASH_ARRAY = 283;
const WARNING_THRESHOLD = 10;
const ALERT_THRESHOLD = 5;

const COLOR_CODES = {
  info: {
    color: "green"
  },
  warning: {
    color: "orange",
    threshold: WARNING_THRESHOLD
  },
  alert: {
    color: "red",
    threshold: ALERT_THRESHOLD
  }
};

const TIME_LIMIT = 30;

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
        streak: 0,
        timePassed: 0,
        timerInterval: null,

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
    /*
     *  Start Game function
     *  Starts the whole game process off. Runs once, takes no parameters.
     */
    startGame() {
      console.log("YAY");
      this.getQs();
    },
    /*
     *  Get Game Details function
     *  This function gets the player's info so that their score and details can be pulled from the database
     */
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
    /*
     *  Check Answer function
     *  Called when the player clicks any of the four answer buttons, it checks if the button pressed contained the correct answer to the
     *  question, returns the appropriate message, adjusts their score and gets the next question.
     */
    checkAnswer(buttonId) {
      console.info(document.getElementById(buttonId).innerHTML)
      if (document.getElementById(buttonId).innerHTML == this.currQuestionJSON.correct_answer) {
        alert("Correct answer! ✔");
        this.streak+=1;
        this.updatePlayerScoreAndPos(((30-(this.timePassed))*10)*(this.streak));

      }
      else {
        alert("Wrong answer! ❌");
        this.streak==1;
        this.updatePlayerScoreAndPos(-500);
      }
      this.getNextQuestion();
    },
    /*
     *  Update Lobby Table function
     *  Updates the database with the questions generated in the front end so players can pull that data when they want to play
     *  TODO: the PUT requests need to be fixed API side, so this function will need amending at some point
     */
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
    /*
     *  Update Player Table function
     *  Updates the database with the Player's info, such as their score, what question they're up to etc.
     *  Currently, it sends everything back to the database, rather than just what needs updating.
     */
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
    /*
     *  Update Player Score and Position function
     *
     *  This function has two purposes - adjusting the player's score and updating the score, position and leaderboard elements.
     *  If it is run at the start of the game, it will query the database for the current player's score and save that as their existing score.
     *  otherwise, it will adjust their score locally and then update the database with the change and pull the latest changes from the database
     *  It also updates the position shown on screen so the player knows where they place on the leaderboard
     *
     *  TODO: a possible fix to the weird updating bug could be fetching the leaderboard, amending it with the player's new score and then updating the database with the new score.
     *     NB: This shouldn't affect how the leaderboard position function works
     *     (the issue is that the database doesn't get updated fast enough for it to show the new changes, so this would be a good way round that.)
     *  TODO: This function also should set the player's question to whatever is stored in the database
     */
    async updatePlayerScoreAndPos(adjustment) {
      if (adjustment == 0) {
        this.tableData = await fetch(`/quizApi/Players/inlobby/${this.lobbyInfo.id}`).then((res) => res.json());
        this.score = this.findCurrentPlayer(false);
      }
      else {
        this.score += adjustment;
        this.updatePlayerTable();
        this.tableData = await fetch(`/quizApi/Players/inlobby/${this.lobbyInfo.id}`).then((res) => res.json);
      }
      document.getElementById("playerScore").innerHTML = `Score: ${this.score}`;
      document.getElementById("streak").innerHTML = `Streak: ${this.streak}`;
      // this.tableData.push({id: 5050, lifeline5050: true, lifelineSkip: true, lobbyId: 90909090, name: "bethany", score: this.score, questionIndex: this.currQuestion}); // TEMPORARY FIX !! TODO: REMOVE THIS LATER after full API integration
      this.tableData.sort((a,b) => b.score - a.score);
      let playerPos = this.findCurrentPlayer(true);
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
    /*
     *  Find Current Player in the Table function
     *  This function finds the player's data in the latest version of the data pulled from the database.
     *  It can be used to get the player's score or their position in the leaderboard
     */
    findCurrentPlayer(position) {
      for (let playerPos=0; playerPos < this.tableData.length; playerPos++) {
        console.info("in player loop!");
        if (this.tableData[playerPos].name === this.nickname) {
            if (position) {
              playerPos++;
              return playerPos;
            }
            else {
              return this.tableData[playerPos].score;
            }
        }
      }
    },
    /*
     *  Get Next Question function
     *  This function simply gets the next question by iterating the player's question index that is locally stored. It updates the data in the Player table and then loads the new question.
     */
    getNextQuestion() {
      this.currQuestion++;
      this.updatePlayerTable();
      this.loadQs();
    },
    /*
     *  Get Questions from Open Trivia DB API function
     *  This function loads the questions from either the Open Trivia DB or from the Database.
     *  If it loads them from the Open Trivia DB, then it would save them locally and then update the database with the new questions so other players can play using the same question.
     *  Otherwise, it will just continue to load the question the player is currently on. (as the questions will be stored in the lobbyInfo object pulled from the DB)
     */
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
    /*
     *  Decode JSON Data function
     *  This function is responsible for decoding the Base64. It decodes them and stores it in the currQuestionJSON object which stores the JSON data of the current question.
     */
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
    /*
     *  Load Current Question function
     *  This function loads the current question - so it loads the data into the currQuestionJSON object from the lobbyInfo object.
     *  It decodes the question data, updates the question field as well as the difficulty, the topic and the question number fields.
     */
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
      }
      if (this.currQuestion == 6) { // TODO: end of game
        document.location.href = "http://localhost:3000/results";
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
      document.getElementById("questionBox").innerHTML = this.currQuestionJSON.question;  // updates the question box and shows the question to the player
      this.updateQuestion();
    },
    /*
     *  Update Question Buttons function
     *  This function updates the answwwer buttons by randomly selecting one of them to be the correct answer and then populating the remaining buttons with incorrect answers
     *  TODO: remove the info log commands from this function so that the answers aren't given away
     */
    updateQuestion() { // pick a random number between 1 and 4, this will be used to asign the correct answer to a button.
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
      this.startTimer();
    },
    /*
     *  Get a random number function
     *  This simply generates a random number between the given range and returns it.
     *  SOURCE: https://www.freecodecamp.org/news/how-to-use-javascript-math-random-as-a-random-number-generator/
     */
    getRandomNum (min, max) {
      return Math.floor(Math.random() * (max - min + 1)) + min;
    },
    onTimesUp() {
      clearInterval(this.timerInterval);
      this.updatePlayerScoreAndPos(0);
      this.getNextQuestion();
      this.streak == 0;
    },
    startTimer() {
      this.timePassed = 0;
      clearInterval(this.timerInterval);
      this.timerInterval = setInterval(() => (this.timePassed += 1), 1000);
    }

  },
  computed: {
    circleDasharray() {
      return `${(this.timeFraction * FULL_DASH_ARRAY).toFixed(0)} 283`;
    },

    formattedTimeLeft() {
      const timeLeft = this.timeLeft;
      const minutes = Math.floor(timeLeft / 60);
      let seconds = timeLeft % 60;

      if (seconds < 10) {
        seconds = `0${seconds}`;
      }

      return `${minutes}:${seconds}`;
    },

    timeLeft() {
      return TIME_LIMIT - this.timePassed;
    },

    timeFraction() {
      const rawTimeFraction = this.timeLeft / TIME_LIMIT;
      return rawTimeFraction - (1 / TIME_LIMIT) * (1 - rawTimeFraction);
    },

    remainingPathColor() {
      const { alert, warning, info } = COLOR_CODES;

      if (this.timeLeft <= alert.threshold) {
        return alert.color;
      } else if (this.timeLeft <= warning.threshold) {
        return warning.color;
      } else {
        return info.color;
      }
    }
  },

  watch: {
    timeLeft(newValue) {
      if (newValue === 0) {
        this.onTimesUp();
      }
    }
  },
  /*
   *  The starting function!
   *  This function is what is called when the page loads.
   *  The lobby information is loaded locally from the database and the player's nickname and lobby ID field are updated on screen.
   *  This function also gets the score and player info from the DB via update player score and position function.
   */
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

    .base-timer {
  position: fixed;
  top: 960px;
  left: 985px;
  width: 100px;
  height: 100px;
  z-index: 2;
  background-color: white;

  &__svg {
    transform: scaleX(-1);
  }

  &__circle {
    fill: none;
    stroke: none;
  }

  &__path-elapsed {
    stroke-width: 7px;
    stroke: grey;
  }

  &__path-remaining {
    stroke-width: 7px;
    stroke-linecap: round;
    transform: rotate(90deg);
    transform-origin: center;
    transition: 1s linear all;
    fill-rule: nonzero;
    stroke: currentColor;

    &.green {
      color: rgb(65, 184, 131);
    }

    &.orange {
      color: orange;
    }

    &.red {
      color: red;
    }
  }

  &__label {
    position: absolute;
    width: 100px;
    height: 100px;
    top: 0;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 40px;
  }
}
</style>
