<template>
<div>
  <GameMusic />
  <section class="section">
    <div class="columns is-mobile">
      <Header></Header>
    </div>
    <!-- <Button title="How To Play"></Button>
    <Button title="About"></Button> -->
  </section>
</div>
</template>

<script>
import GameMusic from '~/components/GameMusic'
import Header from '~/components/Header'
import BlackButton from '~/components/BlackButton'

export default {
  name: 'HomePage',

  components: {
    Header,
    BlackButton,
    GameMusic
  }

}
</script>

<style>
</style>