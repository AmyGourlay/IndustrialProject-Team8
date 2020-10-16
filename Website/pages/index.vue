<template>
<div>
  <GameMusic />
  <article class="panel is-primary">
  <p class="panel-heading">
    Quizz.io
  </p>
  <p class="panel-tabs">
    <a>How To Play</a>
    <a>About</a>
  </p>
  <div class="panel-block">
    <p class="control has-icons-left">
      <input class="input is-primary" type="text" placeholder="Nickname">
      <span class="icon is-left">
        <i class="fas fa-search" aria-hidden="true"></i>
      </span>
    </p>
  </div>
  <a class="panel-block is-active">
    <span class="panel-icon">

    </span>
    bulma
  </a>
</article>
</div>
  <!-- <section class="section">
    <div class="columns is-mobile">
      <card
        title="Free"
        icon="github"
      >
        Open source on <a href="https://github.com/buefy/buefy">
          GitHub
        </a>
      </card>

      <card
        title="Responsive"
        icon="cellphone-link"
      >
        <b class="has-text-grey">
          Every
        </b> component is responsive
      </card>

      <card
        title="Modern"
        icon="alert-decagram"
      >
        Built with <a href="https://vuejs.org/">
          Vue.js
        </a> and <a href="http://bulma.io/">
          Bulma
        </a>
      </card>

      <card
        title="Lightweight"
        icon="arrange-bring-to-front"
      >
        No other internal dependency
      </card>
    </div>
  </section> -->
</template>

<script>
import GameMusic from '~/components/GameMusic.vue';
import Card from '~/components/Card'
import header from '~/components/header'

export default {
  name: 'HomePage',

  components: {
    Card,
    header,
    GameMusic
  }
}
</script>
