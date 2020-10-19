<template>
  <div class="background">
    <nav
      class="navbar header"
      role="navigation"
      aria-label="main navigation"
    >
      <!--<div class="columns is-4 is-offset-1 navbar-item">
        <div class="tile">
            <article class="tile is-child notification is-white">
              <div>
                <p class="subtitle">Lobby code:</p>
                <p class="title" id="lobbyCode">----</p>
              </div>
              <div>
                <p class="subtitle">Nickname:</p>
                <p class="title" id="userNickname"> ---- </p>
              </div>
            </article>
        </div>
      </div>-->
      <div class="columns navbar-brand">
        <a class="navbar-item" href="/">
          <h1>Quizz.io
            <nobr class="subtitle">Lobby code:</nobr>
            <nobr class="title" id="lobbyCode">----</nobr>
            <nobr class="subtitle">Nickname:</nobr>
            <nobr class="title" id="userNickname">----</nobr>
          </h1>
                
        </a>
      </div>
    </nav>

    <div class="columns is-mobile is-centered">
      <router-link to="/howToPlay"><b-button class="button" size="is-large">How To Play</b-button></router-link>
      <router-link to="/about"><b-button class="button" size="is-large">About Us</b-button></router-link>
    </div>

      <div class="container column is-10">
        <router-view></router-view>
      </div>
  </div>
</template>

<style>

.navbar {
  background-color: transparent;
}
.navbar .navbar-brand {
text-align: center;
display: block;
width: 50%;
margin-top: 25px !important;
margin: 0 auto;
background-color: white;
border: 3px solid black;
}
.navbar .navbar-brand > .navbar-item,
.navbar .navbar-brand .navbar-link {
  display: inline-block;
  font-size: 100px;
  padding-bottom: 20px;
  color: black !important;
}
.button {
  background-color: black;
  color: white;
  width: 250px;
  height: 100px;
  text-align: center;
  font-size: 50px;
}
.background {
  height: 100%;
  width: 100%;
  background-image: url("~assets/Background.svg") !important;
  background-color: #20A39E;
  background-position: 0%, 0%, 50%, 50%;
  background-size: auto, cover;
}
</style>
