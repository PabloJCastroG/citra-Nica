import {
  StyleSheet,
  Text,
  View,
  Image,
  TouchableOpacity,
  Linking,
} from 'react-native';
import { LinearGradient } from 'expo-linear-gradient';
import { useRouter } from 'expo-router';
import { SafeAreaView } from 'react-native-safe-area-context';

export default function WelcomeScreen() {
  const router = useRouter();

  return (
    <LinearGradient colors={['#45C264', '#1AB8A0']} style={styles.gradient}>
      <SafeAreaView style={styles.safe}>
        <View style={styles.container}>
          <View style={styles.logoSection}>
            <Image
              source={require('../assets/images/logo.png')}
              style={styles.logo}
              resizeMode="contain"
            />
            <Text style={styles.brandText}>
              <Text style={styles.brandWhite}>Citra</Text>
              <Text style={styles.brandOrange}>Nica</Text>
            </Text>
            <Text style={styles.tagline}>del campo a tu mesa</Text>
          </View>

          <View style={styles.buttonSection}>
            <TouchableOpacity
              style={styles.loginButton}
              onPress={() => router.push('/login')}
              activeOpacity={0.85}
            >
              <Text style={styles.loginButtonText}>Iniciar Sesión</Text>
            </TouchableOpacity>

            <TouchableOpacity
              style={styles.registerButton}
              onPress={() => router.push('/register')}
              activeOpacity={0.85}
            >
              <Text style={styles.registerButtonText}>Registrate</Text>
            </TouchableOpacity>

            <Text style={styles.orgText}>
              Crear cuenta de organización o institución{' '}
              <Text
                style={styles.orgLink}
                onPress={() =>
                  router.push('/(auth)/register-organization/step-1')
                }
              >
                ingresa aquí
              </Text>
            </Text>
          </View>
        </View>
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
  container: {
    flex: 1,
    paddingHorizontal: 32,
    justifyContent: 'space-between',
    paddingTop: 48,
    paddingBottom: 32,
  },
  logoSection: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
  },
  logo: {
    width: 220,
    height: 220,
    marginBottom: 16,
  },
  brandText: {
    fontSize: 48,
    fontWeight: '800',
    letterSpacing: 1,
    marginBottom: 8,
  },
  brandWhite: {
    color: '#FFFFFF',
  },
  brandOrange: {
    color: '#E87722',
  },
  tagline: {
    fontSize: 16,
    color: 'rgba(255,255,255,0.90)',
    fontWeight: '400',
    letterSpacing: 0.3,
  },
  buttonSection: {
    gap: 14,
    alignItems: 'center',
  },
  loginButton: {
    width: '100%',
    backgroundColor: '#FFFFFF',
    borderRadius: 50,
    paddingVertical: 16,
    alignItems: 'center',
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.12,
    shadowRadius: 6,
    elevation: 4,
  },
  loginButtonText: {
    color: '#1AB8A0',
    fontSize: 17,
    fontWeight: '700',
    letterSpacing: 0.3,
  },
  registerButton: {
    width: '100%',
    backgroundColor: '#E87722',
    borderRadius: 50,
    paddingVertical: 16,
    alignItems: 'center',
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.15,
    shadowRadius: 6,
    elevation: 4,
  },
  registerButtonText: {
    color: '#FFFFFF',
    fontSize: 17,
    fontWeight: '700',
    letterSpacing: 0.3,
  },
  orgText: {
    fontSize: 13,
    color: 'rgba(255,255,255,0.85)',
    textAlign: 'center',
    marginTop: 4,
  },
  orgLink: {
    color: '#E87722',
    fontWeight: '600',
  },
});
