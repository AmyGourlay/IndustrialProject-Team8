<template>
    <article class="panel is-primary">
  <p class="panel-heading">
    Primary
  </p>
  <p class="panel-tabs">
    <a class="is-active">All</a>
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

</template>

<script>
    export default {
        name: "header",
        methods: {
            clickMe() {
                this.$buefy.notification.open('Clicked!!')
            }
        }
    }
</script>

