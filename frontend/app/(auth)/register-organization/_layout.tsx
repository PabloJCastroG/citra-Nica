import { Stack } from 'expo-router';

export default function RegisterOrganizationLayout() {
  return (
    <Stack screenOptions={{ headerShown: false }}>
      <Stack.Screen name="step-1" />
      <Stack.Screen name="step-2" />
      <Stack.Screen name="success" options={{ gestureEnabled: false }} />
    </Stack>
  );
}
