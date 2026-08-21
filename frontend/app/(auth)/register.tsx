import { useState } from 'react';
import {
  StyleSheet,
  Text,
  View,
  TextInput,
  TouchableOpacity,
  KeyboardAvoidingView,
  Platform,
  ScrollView,
} from 'react-native';
import { LinearGradient } from 'expo-linear-gradient';
import { SafeAreaView } from 'react-native-safe-area-context';
import { useRouter } from 'expo-router';
import { Eye, EyeOff, ArrowLeft } from 'lucide-react-native';

export default function RegisterScreen() {
  const router = useRouter();
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirm, setShowConfirm] = useState(false);
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const handleRegister = async () => {
    setError('');
    if (
      !name.trim() ||
      !email.trim() ||
      !password.trim() ||
      !confirmPassword.trim()
    ) {
      setError('Por favor completa todos los campos.');
      return;
    }
    if (password !== confirmPassword) {
      setError('Las contraseñas no coinciden.');
      return;
    }
    if (password.length < 8) {
      setError('La contraseña debe tener al menos 8 caracteres.');
      return;
    }

    setLoading(true);

    // Simulación de carga y redirección
    setTimeout(() => {
      setLoading(false);
      router.push('/(auth)/newregister');
    }, 1200);
  };

  return (
    <LinearGradient colors={['#45C264', '#1AB8A0']} style={styles.gradient}>
      <SafeAreaView style={styles.safe}>
        <KeyboardAvoidingView
          style={styles.keyboardView}
          behavior={Platform.OS === 'ios' ? 'padding' : 'height'}
        >
          <ScrollView
            contentContainerStyle={styles.scroll}
            keyboardShouldPersistTaps="handled"
            showsVerticalScrollIndicator={false}
          >
            <TouchableOpacity
              style={styles.backButton}
              onPress={() => router.back()}
            >
              <ArrowLeft color="#FFFFFF" size={24} strokeWidth={2.5} />
            </TouchableOpacity>

            <View style={styles.header}>
              <Text style={styles.title}>
                <Text style={styles.titleWhite}>Citra</Text>
                <Text style={styles.titleOrange}>Nica</Text>
              </Text>
              <Text style={styles.subtitle}>Crea tu cuenta</Text>
            </View>

            <View style={styles.card}>
              <Text style={styles.cardTitle}>Regístrate</Text>

              {error ? (
                <View style={styles.errorBox}>
                  <Text style={styles.errorText}>{error}</Text>
                </View>
              ) : null}

              <View style={styles.fieldGroup}>
                <Text style={styles.label}>Nombre completo</Text>
                <TextInput
                  style={styles.input}
                  placeholder="Tu nombre completo"
                  placeholderTextColor="#A0AEC0"
                  value={name}
                  onChangeText={setName}
                  autoCapitalize="words"
                />
              </View>

              <View style={styles.fieldGroup}>
                <Text style={styles.label}>Correo electrónico</Text>
                <TextInput
                  style={styles.input}
                  placeholder="correo@ejemplo.com"
                  placeholderTextColor="#A0AEC0"
                  value={email}
                  onChangeText={setEmail}
                  keyboardType="email-address"
                  autoCapitalize="none"
                  autoCorrect={false}
                />
              </View>

              <View style={styles.fieldGroup}>
                <Text style={styles.label}>Contraseña</Text>
                <View style={styles.passwordRow}>
                  <TextInput
                    style={styles.passwordInput}
                    placeholder="Mínimo 8 caracteres"
                    placeholderTextColor="#A0AEC0"
                    value={password}
                    onChangeText={setPassword}
                    secureTextEntry={!showPassword}
                    autoCapitalize="none"
                  />
                  <TouchableOpacity
                    style={styles.eyeButton}
                    onPress={() => setShowPassword((v) => !v)}
                    activeOpacity={0.7}
                  >
                    {showPassword ? (
                      <EyeOff color="#718096" size={20} />
                    ) : (
                      <Eye color="#718096" size={20} />
                    )}
                  </TouchableOpacity>
                </View>
              </View>

              <View style={styles.fieldGroup}>
                <Text style={styles.label}>Confirmar contraseña</Text>
                <View style={styles.passwordRow}>
                  <TextInput
                    style={styles.passwordInput}
                    placeholder="Repite tu contraseña"
                    placeholderTextColor="#A0AEC0"
                    value={confirmPassword}
                    onChangeText={setConfirmPassword}
                    secureTextEntry={!showConfirm}
                    autoCapitalize="none"
                  />
                  <TouchableOpacity
                    style={styles.eyeButton}
                    onPress={() => setShowConfirm((v) => !v)}
                    activeOpacity={0.7}
                  >
                    {showConfirm ? (
                      <EyeOff color="#718096" size={20} />
                    ) : (
                      <Eye color="#718096" size={20} />
                    )}
                  </TouchableOpacity>
                </View>
              </View>

              <TouchableOpacity
                style={[
                  styles.registerButton,
                  loading && styles.buttonDisabled,
                ]}
                onPress={handleRegister}
                activeOpacity={0.85}
                disabled={loading}
              >
                <Text style={styles.registerButtonText}>
                  {loading ? 'Creando cuenta...' : 'Crear Cuenta'}
                </Text>
              </TouchableOpacity>

              <View style={styles.loginRow}>
                <Text style={styles.loginText}>¿Ya tienes cuenta? </Text>
                <TouchableOpacity
                  onPress={() => router.push('/login')}
                  activeOpacity={0.7}
                >
                  <Text style={styles.loginLink}>Inicia sesión</Text>
                </TouchableOpacity>
              </View>
            </View>
          </ScrollView>
        </KeyboardAvoidingView>
      </SafeAreaView>
    </LinearGradient>
  );
}

const styles = StyleSheet.create({
  gradient: {
    flex: 1,
  },
  safe: {
    flex: 1,
  },
  keyboardView: {
    flex: 1,
  },
  scroll: {
    flexGrow: 1,
    paddingHorizontal: 24,
    paddingBottom: 32,
  },
  backButton: {
    marginTop: 8,
    marginBottom: 16,
    width: 40,
    height: 40,
    justifyContent: 'center',
  },
  header: {
    alignItems: 'center',
    marginBottom: 28,
  },
  title: {
    fontSize: 40,
    fontWeight: '800',
    letterSpacing: 1,
    marginBottom: 6,
  },
  titleWhite: {
    color: '#FFFFFF',
  },
  titleOrange: {
    color: '#E87722',
  },
  subtitle: {
    fontSize: 15,
    color: 'rgba(255,255,255,0.88)',
    fontWeight: '400',
  },
  card: {
    backgroundColor: '#FFFFFF',
    borderRadius: 24,
    padding: 28,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 8 },
    shadowOpacity: 0.12,
    shadowRadius: 16,
    elevation: 8,
  },
  cardTitle: {
    fontSize: 22,
    fontWeight: '700',
    color: '#1A2E2B',
    marginBottom: 20,
    textAlign: 'center',
  },
  errorBox: {
    backgroundColor: '#FFF0EE',
    borderRadius: 10,
    padding: 12,
    marginBottom: 16,
    borderWidth: 1,
    borderColor: '#FECACA',
  },
  errorText: {
    color: '#DC2626',
    fontSize: 13,
    textAlign: 'center',
  },
  fieldGroup: {
    marginBottom: 16,
  },
  label: {
    fontSize: 13,
    fontWeight: '600',
    color: '#4A5568',
    marginBottom: 6,
  },
  input: {
    borderWidth: 1.5,
    borderColor: '#E2E8F0',
    borderRadius: 12,
    paddingHorizontal: 14,
    paddingVertical: 13,
    fontSize: 15,
    color: '#1A2E2B',
    backgroundColor: '#F7FAFC',
  },
  passwordRow: {
    flexDirection: 'row',
    alignItems: 'center',
    borderWidth: 1.5,
    borderColor: '#E2E8F0',
    borderRadius: 12,
    backgroundColor: '#F7FAFC',
  },
  passwordInput: {
    flex: 1,
    paddingHorizontal: 14,
    paddingVertical: 13,
    fontSize: 15,
    color: '#1A2E2B',
  },
  eyeButton: {
    paddingHorizontal: 14,
    paddingVertical: 13,
  },
  registerButton: {
    backgroundColor: '#E87722',
    borderRadius: 50,
    paddingVertical: 15,
    alignItems: 'center',
    marginTop: 4,
    shadowColor: '#E87722',
    shadowOffset: { width: 0, height: 4 },
    shadowOpacity: 0.3,
    shadowRadius: 8,
    elevation: 4,
  },
  buttonDisabled: {
    opacity: 0.7,
  },
  registerButtonText: {
    color: '#FFFFFF',
    fontSize: 16,
    fontWeight: '700',
    letterSpacing: 0.3,
  },
  loginRow: {
    flexDirection: 'row',
    justifyContent: 'center',
    marginTop: 20,
  },
  loginText: {
    fontSize: 14,
    color: '#718096',
  },
  loginLink: {
    fontSize: 14,
    color: '#1AB8A0',
    fontWeight: '700',
  },
});
