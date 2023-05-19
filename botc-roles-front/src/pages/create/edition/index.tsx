import { Fragment, useEffect, useState } from "react";
import Title from "@/components/ui/title";
import { Button, Container, Input, Spacer } from "@nextui-org/react";
import { createNewEdition, getAllEditions } from "../../../../data/back-api";
import { Text } from "@nextui-org/react";
import classes from "../index.module.css";
import { Check, XOctagon } from "react-feather";
import { Role } from "@/entities/Role";
import { toLowerRemoveDiacritics } from "@/helper/string";
import RolesSelector from "@/components/roles-selector/RolesSelector";

export default function CreateEdition() {
  const [inputKey, setInputKey] = useState(0);
  const [editionName, setEditionName] = useState("");
  const [message, setMessage] = useState(<Fragment />);
  const [selectedRoles, setSelectedRoles] = useState<Role[]>([]);

  const [editions, setEditions] = useState<string[]>([]);
  useEffect(() => {
    async function initEditions() {
      const tempEditions = (await getAllEditions()).map((edition) => {
        return edition.name;
      });
      setEditions(tempEditions);
    }
    initEditions();
  }, []);

  const title = <Title>Création d{"'"}un nouveau module</Title>;

  async function createEdition() {
    if (
      await createNewEdition(
        editionName,
        selectedRoles.map((sr) => sr.id)
      )
    ) {
      editions.push(editionName);
      setEditionName("");
      setEditions(editions);

      updateMessage(false, `Module "${editionName}" enregistré correctement.`);
      setInputKey(inputKey + 1);
      setSelectedRoles([]);
    } else {
      //Erreur
      updateMessage(
        true,
        "Une erreur est survenue lors de l'enregistrement du module."
      );
    }
  }

  function updateMessage(isError: boolean, message: string) {
    if (isError) {
      setMessage(
        <Text span className={classes.red}>
          <XOctagon className={classes.icon} />
          {message}
        </Text>
      );
    } else {
      setMessage(
        <Text span className={classes.green}>
          <Check className={classes.icon} />
          {message}
        </Text>
      );
    }
  }

  function canCreateEdition() {
    // Can create a edition when
    //  - the name is set
    //  - the name is unique
    return (
      editionName !== "" &&
      editions.filter(
        (p) =>
          toLowerRemoveDiacritics(p) === toLowerRemoveDiacritics(editionName)
      ).length === 0
    );
  }

  function editionNameChanged(value: string) {
    setEditionName(value);
    checkError(value);
  }

  function checkError(editionName: string) {
    setMessage(<Fragment />);

    if (editionName.trim() === "") {
      updateMessage(true, "Le nom du module ne doit pas être vide.");
      return;
    }

    const editionsWithSameName = editions.filter(
      (p) => toLowerRemoveDiacritics(p) === toLowerRemoveDiacritics(editionName)
    );
    if (editionsWithSameName.length !== 0) {
      updateMessage(
        true,
        `Le module '${editionsWithSameName[0]}' existe déjà.`
      );
      return;
    }
  }

  return (
    <Fragment>
      {title}
      <Spacer y={2} />
      {message}
      <Spacer y={2} />
      <Container fluid css={{ display: "flex", flexDirection: "column" }}>
        <Input
          key={inputKey}
          clearable
          bordered
          labelPlaceholder="Nom"
          aria-label="Nom"
          onChange={(event) => editionNameChanged(event.target.value)}
        />
        <Spacer y={3} />
        <RolesSelector
          selectedRoles={selectedRoles}
          setSelectedRoles={setSelectedRoles}
        />
        <Spacer y={3} />
      </Container>

      <Button
        shadow
        ghost
        color="success"
        onPress={createEdition}
        disabled={!canCreateEdition()}
      >
        Créer un module
      </Button>
      <Spacer y={3} />
    </Fragment>
  );
}
