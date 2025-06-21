<template>
  <div class="home-container">
    <div class="hero-section">
      <div class="hero-content">
        <h1 class="hero-title">SpecialMoment</h1>
        <p class="hero-subtitle">Planifică evenimentele tale speciale cu ușurință</p>
        <p class="hero-description">
          Nunti, zile de naștere, botezuri și multe altele. 
          Organizează-ți evenimentul perfect cu ajutorul nostru.
        </p>
        <div class="hero-actions">
          <Button 
            @click="goToLogin" 
            label="Conectare" 
            severity="primary" 
            size="large"
            class="hero-button primary-btn"
          />
          <Button 
            @click="goToRegister" 
            label="Înregistrare" 
            severity="secondary" 
            size="large"
            class="hero-button secondary-btn"
          />
        </div>
      </div>
      <div class="hero-image">
        <i class="pi pi-calendar-plus hero-icon"></i>
        <div class="floating-elements">
          <div class="floating-heart">💕</div>
          <div class="floating-star">✨</div>
          <div class="floating-flower">🌸</div>
          <div class="floating-heart-2">💖</div>
        </div>
      </div>
    </div>
    
    <div class="features-section">
      <h2>De ce să alegi SpecialMoment?</h2>
      <div class="features-grid">
        <Card class="feature-card">
          <template #title>
            <i class="pi pi-calendar"></i>
            Planificare Simplă
          </template>
          <template #content>
            <p>Planifică-ți evenimentul pas cu pas cu ajutorul ghidurilor noastre interactive.</p>
          </template>
        </Card>
        
        <Card class="feature-card">
          <template #title>
            <i class="pi pi-users"></i>
            Gestionare Invitați
          </template>
          <template #content>
            <p>Gestionează lista de invitați, trimite invitații și urmărește răspunsurile.</p>
          </template>
        </Card>
        
        <Card class="feature-card">
          <template #title>
            <i class="pi pi-clock"></i>
            Timeline Eveniment
          </template>
          <template #content>
            <p>Creează un timeline detaliat pentru evenimentul tău cu toate activitățile.</p>
          </template>
        </Card>
      </div>
    </div>
    
    <div class="event-types-section">
      <h2>Tipuri de Evenimente</h2>
      <div class="event-types-grid">
        <div class="event-type" v-for="eventType in eventTypes" :key="eventType.id">
          <i :class="eventType.icon"></i>
          <h3>{{ eventType.title }}</h3>
          <p>{{ eventType.description }}</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
// Importurile sunt automatizate de unplugin-auto-import și unplugin-vue-components
const router = useRouter()

const eventTypes = [
  {
    id: 1,
    title: 'Nunti',
    description: 'Planifică-ți ziua cea mai specială cu ajutorul nostru',
    icon: 'pi pi-heart'
  },
  {
    id: 2,
    title: 'Zile de Naștere',
    description: 'Organizează petreceri memorabile pentru toate vârstele',
    icon: 'pi pi-gift'
  },
  {
    id: 3,
    title: 'Botezuri',
    description: 'Celebrează momentul special al familiei tale',
    icon: 'pi pi-star'
  },
  {
    id: 4,
    title: 'Aniversări',
    description: 'Comemorează momentele importante din viața ta',
    icon: 'pi pi-calendar-times'
  }
]

const goToLogin = () => {
  router.push('/login')
}

const goToRegister = () => {
  router.push('/register')
}
</script>

<style scoped>
.home-container {
  min-height: 100vh;
  width: 100vw;
  background: linear-gradient(135deg, #ff6b9d 0%, #c44569 25%, #f8b5d3 50%, #ff9ff3 75%, #f368e0 100%);
  overflow-x: hidden;
}

.hero-section {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 4rem 2rem;
  color: white;
  max-width: 1200px;
  margin: 0 auto;
  min-height: 100vh;
  position: relative;
}

.hero-content {
  flex: 1;
  max-width: 600px;
  z-index: 2;
}

.hero-title {
  font-size: 4rem;
  font-weight: 700;
  margin-bottom: 1.5rem;
  font-family: 'Julee', cursive;
  background: linear-gradient(135deg, #ffffff, #f8b5d3);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
  text-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

.hero-subtitle {
  font-size: 1.8rem;
  margin-bottom: 1.5rem;
  opacity: 0.95;
  font-weight: 600;
}

.hero-description {
  font-size: 1.2rem;
  margin-bottom: 3rem;
  opacity: 0.9;
  line-height: 1.8;
  font-weight: 500;
}

.hero-actions {
  display: flex;
  gap: 1.5rem;
  flex-wrap: wrap;
}

.hero-button {
  padding: 15px 30px;
  font-size: 1.2rem;
  font-weight: 600;
  border-radius: 15px;
  transition: all 0.3s ease;
  box-shadow: 0 8px 20px rgba(0, 0, 0, 0.2);
}

.primary-btn {
  background: linear-gradient(135deg, #c44569, #ff6b9d);
  border: none;
}

.primary-btn:hover {
  transform: translateY(-3px);
  box-shadow: 0 12px 25px rgba(196, 69, 105, 0.4);
  background: linear-gradient(135deg, #ff6b9d, #c44569);
}

.secondary-btn {
  background: rgba(255, 255, 255, 0.2);
  border: 2px solid rgba(255, 255, 255, 0.3);
  backdrop-filter: blur(10px);
}

.secondary-btn:hover {
  transform: translateY(-3px);
  background: rgba(255, 255, 255, 0.3);
  box-shadow: 0 12px 25px rgba(255, 255, 255, 0.2);
}

.hero-image {
  flex: 1;
  display: flex;
  justify-content: center;
  align-items: center;
  position: relative;
  z-index: 1;
}

.hero-icon {
  font-size: 18rem;
  opacity: 0.4;
  color: white;
  filter: drop-shadow(0 8px 16px rgba(0, 0, 0, 0.2));
}

.floating-elements {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  pointer-events: none;
}

.floating-heart, .floating-star, .floating-flower, .floating-heart-2 {
  position: absolute;
  font-size: 3rem;
  animation: float 8s ease-in-out infinite;
  opacity: 0.4;
}

.floating-heart {
  top: 20%;
  right: 20%;
  animation-delay: 0s;
}

.floating-star {
  bottom: 30%;
  right: 10%;
  animation-delay: 2s;
}

.floating-flower {
  top: 60%;
  right: 30%;
  animation-delay: 4s;
}

.floating-heart-2 {
  bottom: 20%;
  right: 40%;
  animation-delay: 6s;
}

@keyframes float {
  0%, 100% { transform: translateY(0px) rotate(0deg); }
  50% { transform: translateY(-30px) rotate(180deg); }
}

.features-section {
  background: rgba(255, 255, 255, 0.95);
  padding: 5rem 2rem;
  text-align: center;
  backdrop-filter: blur(20px);
}

.features-section h2 {
  font-size: 3rem;
  color: #2c3e50;
  margin-bottom: 4rem;
  font-family: 'Julee', cursive;
  background: linear-gradient(135deg, #c44569, #ff6b9d);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
}

.features-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(350px, 1fr));
  gap: 3rem;
  max-width: 1200px;
  margin: 0 auto;
}

.feature-card {
  text-align: center;
  transition: all 0.3s ease;
  border-radius: 20px;
  overflow: hidden;
  box-shadow: 0 10px 30px rgba(196, 69, 105, 0.1);
  border: 1px solid rgba(248, 181, 211, 0.3);
}

.feature-card:hover {
  transform: translateY(-10px);
  box-shadow: 0 20px 40px rgba(196, 69, 105, 0.2);
}

.feature-card :deep(.p-card-title) {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 1rem;
  color: #c44569;
  font-weight: 700;
  font-size: 1.4rem;
  padding: 1.5rem 1.5rem 0.5rem;
}

.feature-card :deep(.p-card-title i) {
  font-size: 2rem;
  background: linear-gradient(135deg, #ff6b9d, #c44569);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
}

.feature-card :deep(.p-card-content) {
  padding: 0 1.5rem 1.5rem;
  color: #7f8c8d;
  font-size: 1.1rem;
  line-height: 1.6;
}

.event-types-section {
  background: linear-gradient(135deg, #f8b5d3 0%, #ff9ff3 50%, #f368e0 100%);
  padding: 5rem 2rem;
  text-align: center;
}

.event-types-section h2 {
  font-size: 3rem;
  color: white;
  margin-bottom: 4rem;
  font-family: 'Julee', cursive;
  text-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

.event-types-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
  gap: 2.5rem;
  max-width: 1200px;
  margin: 0 auto;
}

.event-type {
  background: rgba(255, 255, 255, 0.95);
  padding: 3rem 2rem;
  border-radius: 20px;
  box-shadow: 0 15px 35px rgba(0, 0, 0, 0.1);
  transition: all 0.3s ease;
  backdrop-filter: blur(10px);
  border: 1px solid rgba(255, 255, 255, 0.3);
}

.event-type:hover {
  transform: translateY(-8px);
  box-shadow: 0 25px 50px rgba(0, 0, 0, 0.15);
  background: rgba(255, 255, 255, 1);
}

.event-type i {
  font-size: 4rem;
  color: #c44569;
  margin-bottom: 1.5rem;
  background: linear-gradient(135deg, #ff6b9d, #c44569);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
}

.event-type h3 {
  font-size: 1.8rem;
  color: #2c3e50;
  margin-bottom: 1rem;
  font-weight: 700;
}

.event-type p {
  color: #7f8c8d;
  line-height: 1.7;
  font-size: 1.1rem;
  font-weight: 500;
}

/* Responsive Design */
@media (max-width: 1024px) {
  .hero-section {
    padding: 3rem 1.5rem;
  }
  
  .hero-title {
    font-size: 3.5rem;
  }
  
  .hero-subtitle {
    font-size: 1.6rem;
  }
  
  .hero-icon {
    font-size: 15rem;
  }
  
  .features-section,
  .event-types-section {
    padding: 4rem 1.5rem;
  }
  
  .features-section h2,
  .event-types-section h2 {
    font-size: 2.5rem;
  }
}

@media (max-width: 768px) {
  .hero-section {
    flex-direction: column;
    text-align: center;
    padding: 2rem 1rem;
    min-height: 100vh;
  }
  
  .hero-title {
    font-size: 3rem;
  }
  
  .hero-subtitle {
    font-size: 1.4rem;
  }
  
  .hero-description {
    font-size: 1.1rem;
  }
  
  .hero-icon {
    font-size: 12rem;
    margin-top: 2rem;
  }
  
  .hero-actions {
    justify-content: center;
    gap: 1rem;
  }
  
  .hero-button {
    padding: 12px 24px;
    font-size: 1.1rem;
  }
  
  .features-section,
  .event-types-section {
    padding: 3rem 1rem;
  }
  
  .features-section h2,
  .event-types-section h2 {
    font-size: 2.2rem;
    margin-bottom: 3rem;
  }
  
  .features-grid {
    grid-template-columns: 1fr;
    gap: 2rem;
  }
  
  .event-types-grid {
    grid-template-columns: 1fr;
    gap: 2rem;
  }
  
  .floating-elements {
    display: none;
  }
}

@media (max-width: 480px) {
  .hero-section {
    padding: 1.5rem 0.5rem;
  }
  
  .hero-title {
    font-size: 2.5rem;
  }
  
  .hero-subtitle {
    font-size: 1.2rem;
  }
  
  .hero-description {
    font-size: 1rem;
    margin-bottom: 2rem;
  }
  
  .hero-icon {
    font-size: 10rem;
  }
  
  .hero-actions {
    flex-direction: column;
    width: 100%;
  }
  
  .hero-button {
    width: 100%;
    padding: 15px;
  }
  
  .features-section,
  .event-types-section {
    padding: 2rem 0.5rem;
  }
  
  .features-section h2,
  .event-types-section h2 {
    font-size: 2rem;
  }
  
  .feature-card,
  .event-type {
    padding: 2rem 1.5rem;
  }
  
  .event-type i {
    font-size: 3rem;
  }
  
  .event-type h3 {
    font-size: 1.5rem;
  }
}

@media (min-width: 1400px) {
  .hero-section {
    max-width: 1400px;
  }
  
  .hero-title {
    font-size: 4.5rem;
  }
  
  .hero-icon {
    font-size: 20rem;
  }
  
  .features-grid,
  .event-types-grid {
    max-width: 1400px;
  }
}

/* Landscape orientation for mobile */
@media (max-height: 600px) and (orientation: landscape) {
  .hero-section {
    min-height: 100vh;
    padding: 1rem;
  }
  
  .hero-title {
    font-size: 2.5rem;
    margin-bottom: 1rem;
  }
  
  .hero-subtitle {
    font-size: 1.2rem;
    margin-bottom: 1rem;
  }
  
  .hero-description {
    margin-bottom: 1.5rem;
  }
  
  .hero-icon {
    font-size: 8rem;
  }
}
</style> 