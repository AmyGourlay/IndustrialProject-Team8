<template>
  <section class="section">
    <container>
    <img src="~/assets/volume_on.png" @click="toggle" alt="Toggle Sound" height="120px" width="120px" align="right">
    </container>
    <div class="columns is-mobile">
      <Header></Header>
    </div>
    <!-- <Button title="How To Play"></Button>
    <Button title="About"></Button> -->
  </section>
</template>

<script>
import Header from '~/components/Header'
import BlackButton from '~/components/BlackButton'

export default {
  name: 'HomePage',
  methods:
  {
    toggle()
    {
      
    }
  },

  components: {
    Header,
    BlackButton
  }

}
</script>

<style>
</style>