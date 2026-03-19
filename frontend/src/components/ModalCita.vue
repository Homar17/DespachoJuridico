<template>
    <div class="fixed inset-0 bg-black/60 backdrop-blur-sm flex items-center justify-center p-4 z-50 transition-opacity duration-300">
      <div class="bg-despacho-oscuro text-despacho-fondo w-full max-w-2xl rounded-3xl shadow-2xl overflow-hidden border border-despacho-fondo/10">
        
        <div class="p-8 pb-0 flex justify-between items-start">
          <div>
            <h3 class="text-2xl font-bold tracking-tight">Agendar Asesoría</h3>
            <div class="h-1 w-16 bg-despacho-rojo rounded-full mt-1.5 shadow-[0_0_10px_rgba(128,0,0,0.5)]"></div>
          </div>
          <button @click="$emit('cerrar')" class="text-despacho-fondo/50 hover:text-white transition-colors">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2.5" stroke="currentColor" class="w-6 h-6">
              <path stroke-linecap="round" stroke-linejoin="round" d="M6 18L18 6M6 6l12 12" />
            </svg>
          </button>
        </div>
  
        <form @submit.prevent="enviarFormulario" class="p-8 space-y-6">
          
          <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
            
            <div>
              <label class="block text-xs font-bold uppercase tracking-widest text-despacho-tierra mb-1.5">Fecha de la Cita</label>
              <input 
                v-model="fechaSeleccionada" 
                type="date" 
                :min="fechaMinima"
                :max="fechaMaxima"
                class="w-full bg-white/5 border border-despacho-fondo/20 text-despacho-fondo rounded-xl px-4 py-3 focus:outline-none focus:ring-2 focus:ring-despacho-rojo transition-all [color-scheme:dark]"
                required 
              />
            </div>
  
            <div>
              <label class="block text-xs font-bold uppercase tracking-widest text-despacho-tierra mb-1.5">Hora Disponible</label>
              <select 
                v-model="horaSeleccionada"
                class="w-full bg-white/5 border border-despacho-fondo/20 text-despacho-fondo rounded-xl px-4 py-3 focus:outline-none focus:ring-2 focus:ring-despacho-rojo transition-all appearance-none"
                required
              >
                <option value="" disabled class="bg-despacho-oscuro">Selecciona una hora</option>
                <option 
                  v-for="hora in horasDisponibles" 
                  :key="hora" 
                  :value="hora"
                  class="bg-despacho-oscuro"
                >
                  {{ hora }} hrs
                </option>
              </select>
              <div class="pointer-events-none absolute inset-y-0 right-0 flex items-center px-4 pt-6 text-despacho-fondo/50">
                <svg class="h-4 w-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7"></path></svg>
              </div>
            </div>
          </div>
  
          <div>
            <label class="block text-xs font-bold uppercase tracking-widest text-despacho-tierra mb-1.5">Motivo de la consulta</label>
            <textarea 
              v-model="descripcion" 
              rows="4" 
              placeholder="Breve resumen de tu situación legal..."
              class="w-full bg-white/5 border border-despacho-fondo/20 text-despacho-fondo rounded-xl px-4 py-3 focus:outline-none focus:ring-2 focus:ring-despacho-rojo transition-all placeholder:text-despacho-fondo/20 resize-none"
              required
              maxlength="500"
            ></textarea>
          </div>
  
          <p v-if="errorMsg" class="text-xs font-bold text-red-400 bg-red-950/30 p-3 rounded-xl border border-red-900/50 text-center animate-pulse">
            {{ errorMsg }}
          </p>
  
          <div class="pt-2 flex gap-4">
            <button 
              type="button" 
              @click="$emit('cerrar')" 
              class="flex-1 py-3.5 border border-despacho-fondo/30 text-despacho-fondo/70 font-semibold rounded-xl hover:bg-white/5 hover:text-white transition-all active:scale-95"
            >
              Cancelar
            </button>
            <button 
              type="submit" 
              :disabled="cargando"
              class="flex-1 py-3.5 bg-despacho-rojo text-white font-bold rounded-xl shadow-lg hover:bg-despacho-rojo/90 transition-all active:scale-95 shadow-despacho-rojo/30 disabled:opacity-50 disabled:cursor-not-allowed tracking-wider uppercase"
            >
              {{ cargando ? 'Validando horario...' : 'Confirmar Cita' }}
            </button>
          </div>
        </form>
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref, computed } from 'vue'
  
  const emit = defineEmits(['cerrar', 'agendar'])
  
  const fechaSeleccionada = ref('')
  const horaSeleccionada = ref('')
  const descripcion = ref('')
  const errorMsg = ref('')
  const cargando = ref(false)
  
  // 1. Lógica para limitar la fecha (Mínimo Hoy, Máximo 1 mes)
  const obtenerFechasLimites = () => {
    const hoy = new Date()
    
    // Fecha mínima (Hoy en formato YYYY-MM-DD)
    // Usamos un pequeño ajuste para evitar problemas con zonas horarias
    const offset = hoy.getTimezoneOffset()
    const fechaMin = new Date(hoy.getTime() - (offset*60*1000)).toISOString().split('T')[0]
    
    // Fecha máxima (1 mes después)
    const mesSiguiente = new Date(hoy)
    mesSiguiente.setMonth(mesSiguiente.getMonth() + 1)
    const fechaMax = new Date(mesSiguiente.getTime() - (offset*60*1000)).toISOString().split('T')[0]
  
    return { fechaMin, fechaMax }
  }
  
  const { fechaMin, fechaMax } = obtenerFechasLimites()
  const fechaMinima = ref(fechaMin)
  const fechaMaxima = ref(fechaMax)
  
  // 2. Lógica para limitar las horas (9am a 6pm)
  const horasDisponibles = [
    '09:00', '10:00', '11:00', '12:00', '13:00', 
    '14:00', '15:00', '16:00', '17:00', '18:00'
  ]
  
  const setMensajeError = (mensaje) => {
    errorMsg.value = mensaje
    cargando.value = false
  }
  
  defineExpose({ setMensajeError })
  
  const enviarFormulario = () => {
    errorMsg.value = ''
    
    if (!fechaSeleccionada.value || !horaSeleccionada.value || !descripcion.value) {
      errorMsg.value = "Por favor completa todos los campos."
      return
    }
  
    cargando.value = true
    
    // Fusionamos la fecha y la hora para enviarlas a .NET como un solo DateTime
    // Ejemplo resultante: "2026-03-20T14:00:00"
    const fechaHoraString = `${fechaSeleccionada.value}T${horaSeleccionada.value}:00`
  
    emit('agendar', {
      FechaHora: new Date(fechaHoraString).toISOString(),
      Descripcion: descripcion.value
    })
  }
  </script>
  
  <style scoped>
  /* Ocultamos el calendario nativo feo en algunos navegadores para el input de fecha */
  input[type="date"]::-webkit-calendar-picker-indicator {
      filter: invert(1);
      cursor: pointer;
  }
  </style>