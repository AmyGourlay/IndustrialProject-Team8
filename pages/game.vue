<template>
  <div class="container">
    <!--<section class="section is-small">
      <div class="tile is-ancestor">
        <div class="tile is-parent">
          <div class="tile is-child box">
            <p class="title">Quizz.io 📝</p>
          </div>
        </div>
        <div class="tile is-parent">
          <div class="tile is-child box">
            <p class="title"> xXx_qUizL0rdE_xXx </p>
          </div>
        </div>
        <div class="tile is-parent">
          <article class="tile is-child box">
            <b-button class="is-danger is-large">Leave Game   </b-button>
          </article>
        </div>
      </div>
    </section>
    <section class="section">
      <div class="tile ">
          <article class="tile is-child notification is-primary">
            <p class="title">Question 69</p>
            <p class="subtitle"> Topic - Computers?</p>
            <div class="content">
              birds are government drones
            </div>
          </article>
          <article class="tile is-child is-info notification is-4">
            <p class="title"> MY LAST TRACK JUST WENT VIRAL! </p>
          </article>
        </div>

    </section>-->
    <div class="tile is-ancestor">
  <div class="tile is-parent">
    <article class="tile is-child box">
      <p class="title">Quizz.io</p>
      <p class="subtitle">📝📝📝</p>
    </article>
  </div>
  <div class="tile is-parent">
    <article class="tile is-child box">
      <p class="title"> xXx_qUiZlOrd3_xXx </p>
      <p class="subtitle">Nickname</p>
    </article>
  </div>
  <div class="tile is-parent">
    <article class="tile is-child box">
      <b-button class="is-danger is-large">Leave Game   </b-button>
    </article>
  </div>
</div>

<div class="tile is-ancestor">
  <div class="tile is-parent is-8">
    <article class="tile is-child box">
      <p class="title">Question 69</p>
      <p class="subtitle">Topic: Computers?</p>
      <div class="content">
        <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin ornare magna eros, eu pellentesque tortor vestibulum ut. Maecenas non massa sem. Etiam finibus odio quis feugiat facilisis.</p>
      </div>
    </article>
  </div>
  <div class="tile is-parent">
    <article class="tile is-child box">
      <p class="subtitle">Wondering how your competitors are doing? This is the place for you 👀</p>
      <article class="message">
      <div class="message-header">
        <p>Leaderboard</p>
      </div>
      <div class="message-body">
        <b-table :data="data" :columns="columns"></b-table>
      </div>
    </article>
    </article>
  </div>
</div>
  </div>
</template>

<script>
export default {
  name: 'Game',
  data() {
            return {
                data: [
                    {'nickname': 'LisaSimmons', 'score': '8000'},
                    {'nickname': 'stewart', 'score': '900'},
                    {'nickname': 'mandy', 'score': '2'},
                    {'nickname': 'Richard Smith', 'score': '20'},
                    {'nickname': 'Taylor', 'score': '200'}
                ],
                columns: [
                    {
                        field: 'nickname',
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
                ]
            }
        }
}
</script>

<style lang="scss">
  $warning: #ffba49;
  $link: #20a39e;
  $info: #a4a9ad;
    $danger: #f9b1b1;
  $primary: #23001e;
</style>
