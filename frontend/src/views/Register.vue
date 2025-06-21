<template>
  <div class="register-container">
    <div class="register-background">
      <div class="background-overlay"></div>
      <div class="floating-elements">
        <div class="floating-heart">💕</div>
        <div class="floating-star">✨</div>
        <div class="floating-flower">🌸</div>
        <div class="floating-heart-2">💖</div>
        <div class="floating-star-2">⭐</div>
        <div class="floating-gift">🎁</div>
      </div>
    </div>
    
    <div class="register-card">
      <div class="register-header">
        <div class="logo-container">
          <i class="pi pi-calendar-plus logo-icon"></i>
        </div>
        <h1 class="app-title">SpecialMoment</h1>
        <p class="app-subtitle">Creează-ți contul pentru a începe</p>
      </div>
      
      <form @submit.prevent="handleRegister" class="register-form">
        <div class="form-group">
          <label for="name" class="form-label">Nume complet</label>
          <div class="input-wrapper">
            <i class="pi pi-user input-icon"></i>
            <InputText
              id="name"
              v-model="name"
              type="text"
              placeholder="Introduceți numele complet"
              class="register-input"
              :class="{ 'p-invalid': errors.name }"
              :disabled="userStore.isLoading"
            />
          </div>
          <small v-if="errors.name" class="error-message">{{ errors.name }}</small>
        </div>
        
        <div class="form-group">
          <label for="email" class="form-label">Email</label>
          <div class="input-wrapper">
            <i class="pi pi-envelope input-icon"></i>
            <InputText
              id="email"
              v-model="email"
              type="email"
              placeholder="Introduceți email-ul"
              class="register-input"
              :class="{ 'p-invalid': errors.email }"
              :disabled="userStore.isLoading"
            />
          </div>
          <small v-if="errors.email" class="error-message">{{ errors.email }}</small>
        </div>
        
        <div class="form-group">
          <label for="password" class="form-label">Parolă</label>
          <div class="input-wrapper">
            <i class="pi pi-lock input-icon"></i>
            <Password
              id="password"
              v-model="password"
              placeholder="Introduceți parola"
              class="register-input"
              :class="{ 'p-invalid': errors.password }"
              :disabled="userStore.isLoading"
              :feedback="true"
              toggleMask
            />
          </div>
          <small v-if="errors.password" class="error-message">{{ errors.password }}</small>
        </div>
        
        <div class="form-group">
          <label for="confirmPassword" class="form-label">Confirmă parola</label>
          <div class="input-wrapper">
            <i class="pi pi-lock input-icon"></i>
            <Password
              id="confirmPassword"
              v-model="confirmPassword"
              placeholder="Confirmați parola"
              class="register-input"
              :class="{ 'p-invalid': errors.confirmPassword }"
              :disabled="userStore.isLoading"
              :feedback="false"
              toggleMask
            />
          </div>
          <small v-if="errors.confirmPassword" class="error-message">{{ errors.confirmPassword }}</small>
        </div>
        
        <div class="form-options">
          <div class="terms-checkbox">
            <Checkbox
              v-model="acceptTerms"
              :binary="true"
              inputId="terms"
              :class="{ 'p-invalid': errors.terms }"
            />
            <label for="terms" class="checkbox-label">
              Accept <a href="#" class="terms-link">Termenii și Condițiile</a>
            </label>
          </div>
          <small v-if="errors.terms" class="error-message">{{ errors.terms }}</small>
        </div>
        
        <Button
          type="submit"
          :loading="userStore.isLoading"
          :disabled="!isFormValid"
          class="register-button"
          severity="primary"
        >
          <i class="pi pi-user-plus" v-if="!userStore.isLoading"></i>
          {{ userStore.isLoading ? 'Se înregistrează...' : 'Înregistrare' }}
        </Button>
        
        <div v-if="registerError" class="error-alert">
          <i class="pi pi-exclamation-triangle"></i>
          {{ registerError }}
        </div>
      </form>
      
      <div class="register-footer">
        <p class="login-text">
          Ai deja cont? 
          <a href="#" @click.prevent="goToLogin" class="login-link">Conectează-te</a>
        </p>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useUserStore } from '../stores/user'
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'
const router = useRouter()
const userStore = useUserStore()

const name = ref('')
const email = ref('')
const password = ref('')
const confirmPassword = ref('')
const acceptTerms = ref(false)
const registerError = ref('')

const errors = ref({
  name: '',
  email: '',
  password: '',
  confirmPassword: '',
  terms: ''
})

const isFormValid = computed(() => {
  return name.value.trim() && 
         email.value.trim() && 
         password.value.trim() && 
         confirmPassword.value.trim() && 
         acceptTerms.value &&
         !errors.value.name && 
         !errors.value.email && 
         !errors.value.password && 
         !errors.value.confirmPassword && 
         !errors.value.terms
})

const validateEmail = (email: string) => {
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
  return emailRegex.test(email)
}

const validatePassword = (password: string) => {
  // Minim 8 caractere, cel puțin o literă mare, o literă mică și o cifră
  const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d@$!%*?&]{8,}$/
  return passwordRegex.test(password)
}

const validateForm = () => {
  errors.value = { name: '', email: '', password: '', confirmPassword: '', terms: '' }
  
  if (!name.value.trim()) {
    errors.value.name = 'Numele este obligatoriu'
  } else if (name.value.trim().length < 2) {
    errors.value.name = 'Numele trebuie să aibă cel puțin 2 caractere'
  }
  
  if (!email.value.trim()) {
    errors.value.email = 'Email-ul este obligatoriu'
  } else if (!validateEmail(email.value)) {
    errors.value.email = 'Email-ul nu este valid'
  }
  
  if (!password.value.trim()) {
    errors.value.password = 'Parola este obligatorie'
  } else if (!validatePassword(password.value)) {
    errors.value.password = 'Parola trebuie să aibă cel puțin 8 caractere, o literă mare, o literă mică și o cifră'
  }
  
  if (!confirmPassword.value.trim()) {
    errors.value.confirmPassword = 'Confirmarea parolei este obligatorie'
  } else if (password.value !== confirmPassword.value) {
    errors.value.confirmPassword = 'Parolele nu se potrivesc'
  }
  
  if (!acceptTerms.value) {
    errors.value.terms = 'Trebuie să acceptați termenii și condițiile'
  }
  
  return !Object.values(errors.value).some(error => error !== '')
}

const handleRegister = async () => {
  if (!validateForm()) return
  
  registerError.value = ''
  
  try {
    await userStore.register(name.value, email.value, password.value)
    
    // Redirecționează către login cu mesaj de succes
    router.push('/login?registered=true')
  } catch (error: any) {
    registerError.value = error.message || 'Eroare la înregistrare. Încercați din nou.'
  }
}

const goToLogin = () => {
  router.push('/login')
}
</script>

<style scoped>
.register-container {
  min-height: 100vh;
  width: 100vw;
  display: flex;
  align-items: center;
  justify-content: center;
  position: relative;
  padding: 0;
  overflow: hidden;
}

.register-background {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: linear-gradient(135deg, #ff6b9d 0%, #c44569 25%, #f8b5d3 50%, #ff9ff3 75%, #f368e0 100%);
  z-index: -2;
}

.background-overlay {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(255, 255, 255, 0.1);
  z-index: -1;
}

.floating-elements {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  pointer-events: none;
  z-index: -1;
}

.floating-heart, .floating-star, .floating-flower, .floating-heart-2, .floating-star-2, .floating-gift {
  position: absolute;
  font-size: 2rem;
  animation: float 6s ease-in-out infinite;
  opacity: 0.3;
}

.floating-heart {
  top: 10%;
  left: 10%;
  animation-delay: 0s;
}

.floating-star {
  top: 20%;
  right: 15%;
  animation-delay: 1s;
}

.floating-flower {
  bottom: 20%;
  left: 15%;
  animation-delay: 2s;
}

.floating-heart-2 {
  bottom: 10%;
  right: 10%;
  animation-delay: 3s;
}

.floating-star-2 {
  top: 50%;
  left: 5%;
  animation-delay: 4s;
}

.floating-gift {
  top: 30%;
  right: 5%;
  animation-delay: 5s;
}

@keyframes float {
  0%, 100% { transform: translateY(0px) rotate(0deg); }
  50% { transform: translateY(-20px) rotate(180deg); }
}

.register-card {
  background: rgba(255, 255, 255, 0.95);
  border-radius: 25px;
  box-shadow: 0 25px 50px rgba(196, 69, 105, 0.2);
  padding: 3rem;
  width: 100%;
  max-width: 500px;
  backdrop-filter: blur(20px);
  border: 1px solid rgba(255, 255, 255, 0.3);
  position: relative;
  z-index: 1;
}

.register-card::before {
  content: '';
  position: absolute;
  top: -2px;
  left: -2px;
  right: -2px;
  bottom: -2px;
  background: linear-gradient(45deg, #ff6b9d, #c44569, #f8b5d3, #ff9ff3);
  border-radius: 25px;
  z-index: -1;
  opacity: 0.3;
}

.register-header {
  text-align: center;
  margin-bottom: 2.5rem;
}

.logo-container {
  margin-bottom: 1.5rem;
}

.logo-icon {
  font-size: 4rem;
  color: #c44569;
  background: linear-gradient(135deg, #ff6b9d, #c44569);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
  filter: drop-shadow(0 4px 8px rgba(196, 69, 105, 0.3));
}

.app-title {
  font-size: 2.5rem;
  font-weight: 700;
  color: #2c3e50;
  margin: 0 0 0.5rem 0;
  font-family: 'Julee', cursive;
  background: linear-gradient(135deg, #c44569, #ff6b9d);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
}

.app-subtitle {
  color: #7f8c8d;
  margin: 0;
  font-size: 1.1rem;
  font-weight: 500;
}

.register-form {
  margin-bottom: 2rem;
}

.form-group {
  margin-bottom: 1.8rem;
}

.form-label {
  display: block;
  margin-bottom: 0.8rem;
  font-weight: 600;
  color: #2c3e50;
  font-size: 1rem;
}

.input-wrapper {
  position: relative;
}

.input-icon {
  position: absolute;
  left: 15px;
  top: 50%;
  transform: translateY(-50%);
  color: #c44569;
  z-index: 1;
  font-size: 1.1rem;
}

.register-input {
  width: 100%;
  padding: 15px 15px 15px 50px;
  border: 2px solid #f8b5d3;
  border-radius: 15px;
  font-size: 1rem;
  transition: all 0.3s ease;
  background: rgba(255, 255, 255, 0.9);
}

.register-input:focus {
  border-color: #c44569;
  box-shadow: 0 0 0 4px rgba(196, 69, 105, 0.1);
  background: white;
}

.register-input.p-invalid {
  border-color: #e74c3c;
}

.error-message {
  color: #e74c3c;
  font-size: 0.85rem;
  margin-top: 0.5rem;
  display: block;
  font-weight: 500;
}

.form-options {
  margin-bottom: 2rem;
}

.terms-checkbox {
  display: flex;
  align-items: flex-start;
  gap: 0.8rem;
}

.checkbox-label {
  font-size: 0.95rem;
  color: #7f8c8d;
  cursor: pointer;
  font-weight: 500;
  line-height: 1.4;
}

.terms-link {
  color: #c44569;
  text-decoration: none;
  font-weight: 600;
  transition: color 0.3s ease;
}

.terms-link:hover {
  color: #ff6b9d;
  text-decoration: underline;
}

.register-button {
  width: 100%;
  padding: 15px;
  font-size: 1.1rem;
  font-weight: 600;
  border-radius: 15px;
  background: linear-gradient(135deg, #c44569, #ff6b9d);
  border: none;
  color: white;
  cursor: pointer;
  transition: all 0.3s ease;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.8rem;
  box-shadow: 0 8px 20px rgba(196, 69, 105, 0.3);
}

.register-button:hover:not(:disabled) {
  transform: translateY(-3px);
  box-shadow: 0 12px 25px rgba(196, 69, 105, 0.4);
  background: linear-gradient(135deg, #ff6b9d, #c44569);
}

.register-button:disabled {
  opacity: 0.6;
  cursor: not-allowed;
  transform: none;
}

.error-alert {
  background: linear-gradient(135deg, #ffe6e6, #ffcccc);
  color: #e74c3c;
  padding: 15px;
  border-radius: 12px;
  margin-top: 1.5rem;
  display: flex;
  align-items: center;
  gap: 0.8rem;
  font-size: 0.95rem;
  border: 1px solid #ffb3b3;
}

.register-footer {
  text-align: center;
  padding-top: 2rem;
  border-top: 2px solid #f8b5d3;
}

.login-text {
  color: #7f8c8d;
  margin: 0;
  font-size: 1rem;
  font-weight: 500;
}

.login-link {
  color: #c44569;
  text-decoration: none;
  font-weight: 700;
  transition: color 0.3s ease;
}

.login-link:hover {
  color: #ff6b9d;
  text-decoration: underline;
}

/* Responsive Design */
@media (max-width: 768px) {
  .register-container {
    padding: 0;
    min-height: 100vh;
  }
  
  .register-card {
    padding: 2.5rem 2rem;
    border-radius: 20px;
    max-width: 100%;
    margin: 0 1rem;
  }
  
  .app-title {
    font-size: 2.2rem;
  }
  
  .logo-icon {
    font-size: 3.5rem;
  }
  
  .floating-elements {
    display: none;
  }
}

@media (max-width: 480px) {
  .register-container {
    padding: 0;
  }
  
  .register-card {
    padding: 2rem 1.5rem;
    border-radius: 15px;
    margin: 0 0.5rem;
  }
  
  .app-title {
    font-size: 2rem;
  }
  
  .logo-icon {
    font-size: 3rem;
  }
  
  .register-input {
    padding: 12px 12px 12px 45px;
  }
  
  .input-icon {
    left: 12px;
  }
  
  .register-button {
    padding: 12px;
    font-size: 1rem;
  }
}

@media (min-width: 1200px) {
  .register-card {
    max-width: 550px;
    padding: 3.5rem;
  }
  
  .app-title {
    font-size: 3rem;
  }
  
  .logo-icon {
    font-size: 4.5rem;
  }
}

/* Landscape orientation for mobile */
@media (max-height: 600px) and (orientation: landscape) {
  .register-container {
    padding: 0;
  }
  
  .register-card {
    padding: 2rem;
    max-height: 90vh;
    overflow-y: auto;
  }
  
  .register-header {
    margin-bottom: 1.5rem;
  }
  
  .form-group {
    margin-bottom: 1rem;
  }
}
</style> 