<template>
<div>
  <GameMusic />
  <section class="section">
    <div class="columns is-mobile">
      <!--<Header></Header>-->
    </div>
    <!-- <Button title="How To Play"></Button>
    <Button title="About"></Button> -->
  </section>
  <section class="nickname">
     <b-field>
       <b-input placeholder="Enter Nickname" size="is-large" icon="account" rounded></b-input>
     </b-field>
     <div class="joinButton">
        <nuxt-link to="/lobby"><BlackButton title="Join Lobby"></BlackButton></nuxt-link>
      </div>
      <div class="createButton">
        <nuxt-link to="/lobby"><BlackButton title="Create Lobby"></BlackButton></nuxt-link>
      </div>
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
body
{
  /* background image */
}
.nickname
{
  width: 950px;
  margin-top: 8%;
  margin-left: auto;
  margin-right: auto;
}
.joinButton
{
  margin-top: 30px;
  text-align: center;
}
.createButton
{
  margin-top: 10px;
  text-align: center;
}
</style>