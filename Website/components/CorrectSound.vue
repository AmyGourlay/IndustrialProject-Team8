<template>
    <section class="sound">
        <audio id="audio" src="~/assets/CorrectAnswer.mp3"></audio>
        <b-button @click="clickMe">Play</b-button>
    </section>
</template>

<script>

export default 
{
    name: "CorrectSound",

    methods: {
        clickMe() 
        {
            audio.play();
        }
    }
}
</script>

<style>

</style>