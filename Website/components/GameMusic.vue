<template>
    <section class="sound">
        <audio id="audio" src="~/assets/Quizz.mp3" autoplay loop></audio>
        <img class="img" src="~/assets/volume_on.png" @click="toggle" alt="Toggle Sound" height="120px" width="120px" align="right">
    </section>
</template>

<script>

export default 
{
    name: "GameMusic",

    methods: {
        toggle() 
        {
            if(!audio.paused)
            {
                audio.pause();
            }
            else
            {
                audio.play();
            }
                
        }
    }
}
</script>

<style>

</style>