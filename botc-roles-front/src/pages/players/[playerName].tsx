import { Fragment, useEffect, useState } from "react";
import { Player } from "@/entities/Player";
import { useRouter } from "next/router";
import Container from "@/components/list-stats/Container";
import ListItem from "@/components/list-stats/ListItem";
import Title from "@/components/ui/title";
import PlayerName from "@/components/ui/playerName";
import { Collapse, Loading, Spacer } from "@nextui-org/react";
import ImageIconName from "@/components/ui/image-icon-name";
import { getPlayerByName } from "../../../data/back-api";

export default function PlayerPage() {
  const playerName = useRouter().query.playerName?.toString();
  const [player, setPlayer] = useState<Player>();
  const [rolesPlayed, setRolesPlayed] = useState<
    {
      name: string;
      timesPlayed: number;
      category: string;
    }[]
  >();

  useEffect(() => {
    async function initPlayer() {
      if (playerName === undefined) return;

      const p = await getPlayerByName(playerName);
      setPlayer(p.player);
      setRolesPlayed(p.allRolesPlayed);
    }
    initPlayer();
  }, [playerName]);

  if (!playerName) {
    <Loading />;
    return;
  }

  const title = (
    <Title>
      Détails <PlayerName name={playerName} />
    </Title>
  );

  const playerComponent = player ? (
    <ListItem name="Parties jouées" value={player.nbGamesPlayed} />
  ) : (
    <Loading />
  );

  const rolesComponent = rolesPlayed ? (
    rolesPlayed.map((rp) => (
      <ListItem
        key={rp.name}
        name={
          <ImageIconName
            setNameAtRightOfImage
            name={rp.name}
            category={rp.category}
          />
        }
        value={rp.timesPlayed}
      />
    ))
  ) : (
    <Loading />
  );

  return (
    <Fragment>
      {title}
      <Collapse.Group accordion={false} css={{ w: "100%" }}>
        <Collapse expanded title="Détails généraux">
          <Container>
            {playerComponent}

            {/* <ListItem name="Parties gagnées" value={player.wins} />
            <ListItem name="Parties perdues" value={player.loses} />
            <ListItem name="Parties maléfique" value={player.nbTimesEvil} />
            <ListItem name="Parties gentil" value={player.nbTimesGood} /> */}
          </Container>
        </Collapse>
        <Collapse expanded title="Détails des rôles joués">
          <Container>{rolesComponent}</Container>
        </Collapse>
      </Collapse.Group>
    </Fragment>
  );
}
