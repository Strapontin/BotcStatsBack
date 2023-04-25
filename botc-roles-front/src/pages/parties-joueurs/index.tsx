import Container from "@/components/list-stats/Container";
import ListItem from "@/components/list-stats/ListItem";

export default function GamesPlayedByPlayer() {
  return (
    <div>
      <h1>Nombre de parties/joueurs</h1>
      <Container>
        <ListItem></ListItem>
        <ListItem></ListItem>
        <ListItem></ListItem>
        <ListItem></ListItem>
      </Container>
    </div>
  );
}
