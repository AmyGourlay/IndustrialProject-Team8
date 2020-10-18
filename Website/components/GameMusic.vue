<template>
    <section class="sound">
        <audio id="audio" src="~/assets/Quizz.mp3" autoplay loop></audio>
        <!--<b-button @click="clickMe">Pause</b-button>-->
    </section>
</template>

<script>

export default 
{
    name: "GameMusic",

    methods: {
        clickMe() 
        {
            audio.pause();
        }
    }
}
</script>

<style>

</style>