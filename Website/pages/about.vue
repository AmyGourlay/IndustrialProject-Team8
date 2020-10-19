<template>
    <div class="tile notification is-white">
        <article class="content">
            <h2>Quizz.io</h2>
            <div class="media-content">
                <h3>About Us</h3>
                <p>
                    <br>
                    Lorem Ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.
                </p>
                <p><br> Made with 💖 © Riddlers 2020</p>
                <br>
                <nuxt-link to="/"><BlackButton title="Back"></BlackButton></nuxt-link>
            </div>
        </article>
    </div>
</template>

<script>
import GameMusic from '~/components/GameMusic'
 import BlackButton from '~/components/BlackButton'
    export default {
        components: 
            {
                GameMusic,
                BlackButton
            }
    }
</script>

<style>
.columns.is-mobile
{
    justify-content: center;
}
.section
{
    padding: 1.5rem 1.5rem;
}
.aboutbox
{
    width: 950px;
    height: 188px;
    display: table;
}
.content
{
    margin: 0px;
    justify-content: center;
    text-align: center
}
.content h2 
{
    margin: 0%;
    font: normal normal bold 145px/203px Rockwell;
    letter-spacing: 0px;
    color: #24011E;
}
.media-content
{
    text-align: center;
    margin: 20px 50px;
}
.media-content h3
{
    font: normal normal bold 48px/68px Rockwell;
    letter-spacing: 0px;
    color: #24011E;
    opacity: 1;
}
</style>